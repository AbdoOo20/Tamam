using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Public;

using Tamam.Application.Services.Notifications;

using Tamam.Application.Services.Rejections;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Public;

public class PublicOrderService(IUnitOfWork unitOfWork, IOrderOutcomeService outcomes) : IPublicOrderService
{
    public async Task<Result<PublicOrderDto>> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        var context = await LoadAsync(token, null, cancellationToken);

        if (context.Error is not null)
        {
            return Failure(context.Error);
        }

        var now = DateTime.UtcNow;

        context.Link!.OpenCount++;

        context.Link.FirstOpenedAt ??= now;

        if (context.Link.Purpose == PublicLinkPurpose.OrderConfirmation)
        {
            var request = await LatestConfirmationAsync(context.Order!.Id, cancellationToken);

            if (request is { Status: ConfirmationStatus.Sent or ConfirmationStatus.Delivered })
            {
                request.Status = ConfirmationStatus.Opened;
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PublicOrderDto>.Ok(await BuildAsync(context, cancellationToken));
    }

    public async Task<Result<PublicOrderDto>> ConfirmAsync(string token, PublicConfirmRequest request, CancellationToken cancellationToken = default)
    {
        var context = await LoadAsync(token, PublicLinkPurpose.OrderConfirmation, cancellationToken);

        if (context.Error is not null)
        {
            return Failure(context.Error);
        }

        if (!CanRespond(context))
        {
            return Result<PublicOrderDto>.Conflict("This order was already processed.");
        }

        var order = context.Order!;

        var now = DateTime.UtcNow;

        if (request.ProposedDeliveryDate is { } date)
        {
            order.ExpectedDeliveryDate = date;
        }

        Move(order, OrderStatusType.Confirmed, "Customer confirmed the order.", now);

        var confirmation = await LatestConfirmationAsync(order.Id, cancellationToken);

        if (confirmation is not null)
        {
            confirmation.Status = ConfirmationStatus.Confirmed;

            confirmation.RespondedAt = now;

            confirmation.ProposedDeliveryDate = request.ProposedDeliveryDate;

            confirmation.ResponseNote = request.Note?.Trim();
        }

        context.Link!.UsedAt = now;

        await unitOfWork.Repository<Notification>().AddAsync(NotificationFactory.ForOrder(order, NotificationTypes.OrderConfirmed, "العميل أكد الطلب", $"الطلب {order.OrderNo}"), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PublicOrderDto>.Ok(await BuildAsync(context, cancellationToken), "Order confirmed.");
    }

    public async Task<Result<PublicOrderDto>> RescheduleAsync(string token, PublicRescheduleRequest request, CancellationToken cancellationToken = default)
    {
        var context = await LoadAsync(token, PublicLinkPurpose.OrderConfirmation, cancellationToken);

        if (context.Error is not null)
        {
            return Failure(context.Error);
        }

        if (!CanRespond(context))
        {
            return Result<PublicOrderDto>.Conflict("This order was already processed.");
        }

        var date = request.ProposedDeliveryDate!.Value;

        if (date < DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            return Result<PublicOrderDto>.BadRequest("The proposed delivery date cannot be in the past.");
        }

        var order = context.Order!;

        var now = DateTime.UtcNow;

        order.ExpectedDeliveryDate = date;

        Move(order, OrderStatusType.Confirmed, "Customer rescheduled the delivery.", now);

        var confirmation = await LatestConfirmationAsync(order.Id, cancellationToken);

        if (confirmation is not null)
        {
            confirmation.Status = ConfirmationStatus.Rescheduled;

            confirmation.RespondedAt = now;

            confirmation.ProposedDeliveryDate = date;

            confirmation.ResponseNote = request.Note?.Trim();
        }

        context.Link!.UsedAt = now;

        await unitOfWork.Repository<Notification>().AddAsync(NotificationFactory.ForOrder(order, NotificationTypes.OrderRescheduled, "العميل اقترح موعد توصيل جديد", $"الطلب {order.OrderNo} - {date:yyyy-MM-dd}"), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PublicOrderDto>.Ok(await BuildAsync(context, cancellationToken), "Delivery rescheduled.");
    }

    public async Task<Result<PublicOrderDto>> CancelAsync(string token, PublicCancelRequest request, CancellationToken cancellationToken = default)
    {
        var context = await LoadAsync(token, PublicLinkPurpose.OrderConfirmation, cancellationToken);

        if (context.Error is not null)
        {
            return Failure(context.Error);
        }

        if (!CanRespond(context))
        {
            return Result<PublicOrderDto>.Conflict("This order was already processed.");
        }

        var order = context.Order!;

        var now = DateTime.UtcNow;

        Move(order, OrderStatusType.CancelledByCustomer, "Customer cancelled the order.", now);

        var confirmation = await LatestConfirmationAsync(order.Id, cancellationToken);

        if (confirmation is not null)
        {
            confirmation.Status = ConfirmationStatus.Cancelled;

            confirmation.RespondedAt = now;

            confirmation.ResponseNote = request.Details?.Trim();
        }

        await unitOfWork.Repository<RejectionReport>().AddAsync(new RejectionReport
        {
            Order = order,

            ReporterType = ReporterKind.Customer,

            ReasonId = request.ReasonId,

            Details = request.Details?.Trim()
        }, cancellationToken);

        await outcomes.ApplyFinalResultAsync(order, OrderFinalResult.Cancelled, cancellationToken);

        context.Link!.UsedAt = now;

        await unitOfWork.Repository<Notification>().AddAsync(NotificationFactory.ForOrder(order, NotificationTypes.OrderCancelledByCustomer, "العميل ألغى الطلب", $"الطلب {order.OrderNo}"), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PublicOrderDto>.Ok(await BuildAsync(context, cancellationToken), "Order cancelled.");
    }

    public async Task<Result<PublicOrderDto>> SubmitSurveyAsync(string token, PublicSurveyRequest request, CancellationToken cancellationToken = default)
    {
        var context = await LoadAsync(token, PublicLinkPurpose.RejectionSurvey, cancellationToken);

        if (context.Error is not null)
        {
            return Failure(context.Error);
        }

        if (!CanRespond(context))
        {
            return Result<PublicOrderDto>.Conflict("This survey was already answered.");
        }

        var order = context.Order!;

        await unitOfWork.Repository<RejectionReport>().AddAsync(new RejectionReport
        {
            Order = order,

            ReporterType = ReporterKind.Customer,

            ReasonId = request.ReasonId,

            Details = request.Details?.Trim()
        }, cancellationToken);

        context.Link!.UsedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await outcomes.ResolveRefusalAsync(order, cancellationToken);

        await unitOfWork.Repository<Notification>().AddAsync(NotificationFactory.ForOrder(order, NotificationTypes.RejectionSurveyAnswered, "العميل أجاب على استبيان الرفض", $"الطلب {order.OrderNo}"), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PublicOrderDto>.Ok(await BuildAsync(context, cancellationToken), "Thank you for your feedback.");
    }

    public async Task<Result> OptOutAsync(string token, CancellationToken cancellationToken = default)
    {
        var context = await LoadAsync(token, null, cancellationToken);

        if (context.Error is not null)
        {
            return context.Error;
        }

        var customer = await unitOfWork.Repository<Customer>().GetByIdAsync(context.Order!.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.NotFound("Customer was not found.");
        }

        if (!customer.IsOptedOut)
        {
            var now = DateTime.UtcNow;

            customer.IsOptedOut = true;

            customer.OptedOutAt = now;

            await unitOfWork.Repository<CustomerDataRequest>().AddAsync(new CustomerDataRequest
            {
                CustomerId = customer.Id,

                Type = DataRequestType.OptOut,

                Status = DataRequestStatus.Done,

                RequestedAt = now,

                ProcessedAt = now
            }, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Ok("You will no longer receive messages.");
    }

    private sealed record LinkContext(OrderPublicLink? Link, Order? Order, Merchant? Merchant, Result? Error);

    private async Task<LinkContext> LoadAsync(string token, PublicLinkPurpose? expectedPurpose, CancellationToken cancellationToken)
    {
        var link = await unitOfWork.Repository<OrderPublicLink>().FirstOrDefaultAsync(item => item.Token == token, cancellationToken);

        if (link is null || (expectedPurpose is { } purpose && link.Purpose != purpose))
        {
            return new LinkContext(null, null, null, Result.NotFound("This link is not valid."));
        }

        if (link.ExpiresAt < DateTime.UtcNow)
        {
            return new LinkContext(null, null, null, Result.Gone("This link has expired."));
        }

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == link.OrderId, cancellationToken);

        if (order is null)
        {
            return new LinkContext(null, null, null, Result.NotFound("This link is not valid."));
        }

        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(order.MerchantId, cancellationToken);

        return new LinkContext(link, order, merchant, null);
    }

    private static bool CanRespond(LinkContext context)
    {
        if (context.Link!.UsedAt is not null)
        {
            return false;
        }

        return context.Link.Purpose == PublicLinkPurpose.OrderConfirmation
            ? context.Order!.StatusId is OrderStatusType.New or OrderStatusType.AwaitingConfirmation
            : context.Order!.StatusId == OrderStatusType.Refused;
    }

    private Task<ConfirmationRequest?> LatestConfirmationAsync(long orderId, CancellationToken cancellationToken) =>

    unitOfWork.Repository<ConfirmationRequest>().Query(asNoTracking: false)
        .Where(item => item.OrderId == orderId)
        .OrderByDescending(item => item.AttemptNo)
        .FirstOrDefaultAsync(cancellationToken);

    private static void Move(Order order, OrderStatusType target, string note, DateTime now)
    {
        order.StatusHistory.Add(new OrderStatusHistory
        {
            FromStatusId = order.StatusId,

            ToStatusId = target,

            ChangedByType = ActorType.Customer,

            Note = note,

            ChangedAt = now
        });

        order.StatusId = target;
    }

    private async Task<PublicOrderDto> BuildAsync(LinkContext context, CancellationToken cancellationToken)
    {
        var order = context.Order!;

        var items = await unitOfWork.Repository<OrderItem>().Query()
            .Where(item => item.OrderId == order.Id)
            .OrderBy(item => item.Id)
            .Select(item => new PublicOrderItemDto(item.ProductName, item.Variant, item.Quantity, item.UnitPrice))
            .ToListAsync(cancellationToken);

        return new PublicOrderDto(
            context.Link!.Purpose,

            context.Merchant?.BusinessName ?? string.Empty,

            order.RecipientName,

            order.CurrencyCode,

            order.TotalAmount,

            order.AmountToCollect,

            order.City,

            order.AddressLine,

            order.ExpectedDeliveryDate,

            order.StatusId,

            CanRespond(context),

            context.Link.ExpiresAt,

            items);
    }

    private static Result<PublicOrderDto> Failure(Result error) => error.Status switch
    {
        ResultStatus.Gone => Result<PublicOrderDto>.Gone(error.Message),

        _ => Result<PublicOrderDto>.NotFound(error.Message)
    };
}
