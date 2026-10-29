using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Orders;

using Tamam.Application.Mappings;

using Tamam.Application.Services.Messaging;

using Tamam.Application.Services.Rejections;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Orders;

public class OrderService(
    IUnitOfWork unitOfWork,

    IUserAppService userAppService,

    IOrderCreationService orderCreation,

    IConfirmationService confirmations,

    IOrderOutcomeService outcomes,

    PublicLinkFactory linkFactory) : IOrderService
    {
        private static readonly Dictionary<OrderStatusType, OrderStatusType[]> Transitions = new()
        {
            [OrderStatusType.New] = [OrderStatusType.AwaitingConfirmation, OrderStatusType.Confirmed, OrderStatusType.CancelledByMerchant],

            [OrderStatusType.AwaitingConfirmation] = [OrderStatusType.Confirmed, OrderStatusType.CancelledByMerchant, OrderStatusType.CancelledByCustomer],

            [OrderStatusType.Confirmed] = [OrderStatusType.DepositRequested, OrderStatusType.Shipped, OrderStatusType.CancelledByMerchant, OrderStatusType.CancelledByCustomer],

            [OrderStatusType.DepositRequested] = [OrderStatusType.Confirmed, OrderStatusType.Shipped, OrderStatusType.CancelledByMerchant, OrderStatusType.CancelledByCustomer],

            [OrderStatusType.Shipped] = [OrderStatusType.OutForDelivery, OrderStatusType.Delivered, OrderStatusType.Refused, OrderStatusType.FailedDelivery, OrderStatusType.Returned],

            [OrderStatusType.OutForDelivery] = [OrderStatusType.Delivered, OrderStatusType.Refused, OrderStatusType.FailedDelivery],

            [OrderStatusType.FailedDelivery] = [OrderStatusType.OutForDelivery, OrderStatusType.Shipped, OrderStatusType.Delivered, OrderStatusType.Refused, OrderStatusType.Returned],

            [OrderStatusType.Refused] = [OrderStatusType.Returned]
    };

        private static readonly OrderStatusType[] EditableStatuses =

        [

        OrderStatusType.New,

        OrderStatusType.AwaitingConfirmation,

        OrderStatusType.Confirmed,

        OrderStatusType.DepositRequested

        ];

        private static readonly OrderStatusType[] DeletableStatuses =

        [

        OrderStatusType.New,

        OrderStatusType.CancelledByMerchant,

        OrderStatusType.CancelledByCustomer

        ];

        public async Task<Result<PagedResult<OrderListItemDto>>> GetAllAsync(OrderQuery query, CancellationToken cancellationToken = default)
        {
            var merchantId = userAppService.RequiredMerchantId;

            var orders = unitOfWork.Repository<Order>().Query().Where(order => order.MerchantId == merchantId);

        if (query.StatusId is { } statusId)
        {
            orders = orders.Where(order => order.StatusId == statusId);
        }

        if (query.RiskLevel is { } riskLevel)
        {
            orders = orders.Where(order => order.LatestRiskLevel == riskLevel);
        }

        if (query.CarrierId is { } carrierId)
        {
            orders = orders.Where(order => order.CarrierId == carrierId);
        }

        if (query.From is { } from)
        {
            orders = orders.Where(order => order.PlacedAt >= from);
        }

        if (query.To is { } to)
        {
            var exclusiveEnd = to.Date.AddDays(1);

            orders = orders.Where(order => order.PlacedAt < exclusiveEnd);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            orders = orders.WhereMatchesSearch(query.Search);
        }

        var page = await orders
            .OrderByDescending(order => order.PlacedAt)
            .Select(order => new OrderListItemDto(
            order.Id,

            order.OrderNo,

            order.RecipientName,

            order.RecipientPhoneE164,

            order.City,

            order.CurrencyCode,

            order.TotalAmount,

            order.AmountToCollect,

            order.StatusId,

            order.Status.NameAr,

            order.LatestRiskScore,

            order.LatestRiskLevel,

            order.ExpectedDeliveryDate,

            order.PlacedAt))
                .ToPagedResultAsync(query.Page, query.PageSize, cancellationToken);

        return Result<PagedResult<OrderListItemDto>>.Ok(page);
    }

    public async Task<Result<OrderDetailDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var row = await unitOfWork.Repository<Order>().Query()
            .Where(order => order.Id == id && order.MerchantId == merchantId)
            .Select(order => new { Order = order, StatusName = order.Status.NameAr, CarrierName = order.Carrier != null ? order.Carrier.Name : null })
            .FirstOrDefaultAsync(cancellationToken);

        if (row is null)
        {
            return Result<OrderDetailDto>.NotFound("Order was not found.");
        }

        var entity = row.Order;

        var items = await unitOfWork.Repository<OrderItem>().Query().Where(item => item.OrderId == id).OrderBy(item => item.Id).ToListAsync(cancellationToken);

        var history = await unitOfWork.Repository<OrderStatusHistory>().Query().Where(item => item.OrderId == id).OrderBy(item => item.ChangedAt).ThenBy(item => item.Id).ToListAsync(cancellationToken);

        var attempts = await unitOfWork.Repository<DeliveryAttempt>().Query().Where(item => item.OrderId == id).OrderBy(item => item.AttemptNo).ToListAsync(cancellationToken);

        var assessment = await unitOfWork.Repository<OrderRiskAssessment>().Query().Where(item => item.OrderId == id).OrderByDescending(item => item.CreatedAt).ThenByDescending(item => item.Id).FirstOrDefaultAsync(cancellationToken);

        var risk = assessment is null
            ? null
            : assessment.ToDto(await unitOfWork.Repository<OrderRiskSignal>().Query().Where(signal => signal.AssessmentId == assessment.Id).ToListAsync(cancellationToken));

        var outcome = await unitOfWork.Repository<OrderOutcome>().Query().FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        var confirmation = (await confirmations.GetCurrentAsync(id, cancellationToken)).Data;

        var now = DateTime.UtcNow;

        var survey = await unitOfWork.Repository<OrderPublicLink>().Query()
            .Where(link => link.OrderId == id && link.Purpose == PublicLinkPurpose.RejectionSurvey && link.ExpiresAt > now)
            .OrderByDescending(link => link.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var detail = new OrderDetailDto(
            entity.Id,

            entity.OrderNo,

            entity.StatusId,

            row.StatusName,

            entity.Source,

            entity.IsHistorical,

            entity.RecipientName,

            entity.RecipientPhoneE164,

            entity.AltPhoneE164,

            entity.CountryCode,

            entity.RegionId,

            entity.City,

            entity.AddressLine,

            entity.Landmark,

            entity.Latitude,

            entity.Longitude,

            entity.CurrencyCode,

            entity.ItemsTotal,

            entity.ShippingFee,

            entity.DiscountAmount,

            entity.TotalAmount,

            entity.PaymentMethod,

            entity.DepositAmount,

            entity.AmountToCollect,

            entity.ExpectedDeliveryDate,

            entity.CarrierId,

            row.CarrierName,

            entity.TrackingNo,

            entity.ShippedAt,

            entity.DeliveredAt,

            entity.Notes,

            entity.PlacedAt,

            items.Select(item => item.ToDto()).ToList(),

            history.Select(item => item.ToDto()).ToList(),

            attempts.Select(item => item.ToDto()).ToList(),

            risk,

            confirmation,

            survey is null ? null : linkFactory.BuildUrl(survey.Purpose, survey.Token),

            outcome?.ToDto());

        return Result<OrderDetailDto>.Ok(detail);
    }

    public async Task<Result<OrderDetailDto>> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(merchantId, cancellationToken);

        if (merchant is null)
        {
            return Result<OrderDetailDto>.NotFound("Merchant was not found.");
        }

        var country = await unitOfWork.Repository<Country>().GetByIdAsync(merchant.CountryCode, cancellationToken);

        var created = await orderCreation.CreateAsync(request, merchant, country?.PhoneCode ?? "+", null, OrderSource.EntryPage, false, cancellationToken);

        if (!created.Succeeded)
        {
            return CopyFailure(created);
        }

        var detail = await GetByIdAsync(created.Data, cancellationToken);

        return detail.Succeeded ? Result<OrderDetailDto>.Created(detail.Data!, "Order created.") : detail;
    }

    public async Task<Result<OrderDetailDto>> UpdateAsync(long id, UpdateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result<OrderDetailDto>.NotFound("Order was not found.");
        }

        if (!EditableStatuses.Contains(order.StatusId))
        {
            return Result<OrderDetailDto>.BadRequest("This order can no longer be edited.");
        }

        var country = await unitOfWork.Repository<Country>().GetByIdAsync(order.CountryCode, cancellationToken);

        string? altPhone = null;

        if (!string.IsNullOrWhiteSpace(request.AltPhone))
        {
            altPhone = PhoneHelper.Normalize(request.AltPhone, country?.PhoneCode ?? "+");

            if (altPhone is null)
            {
                return Result<OrderDetailDto>.BadRequest("Alternative phone number is invalid.");
            }
        }

        var total = order.ItemsTotal + request.ShippingFee - request.DiscountAmount;

        if (total < 0)
        {
            return Result<OrderDetailDto>.BadRequest("Discount cannot exceed the order value.");
        }

        if (request.DepositAmount > total)
        {
            return Result<OrderDetailDto>.BadRequest("Deposit cannot exceed the order total.");
        }

        if (request.CarrierId is { } carrierId && !await unitOfWork.Repository<Carrier>().AnyAsync(carrier => carrier.Id == carrierId && (carrier.MerchantId == merchantId || carrier.MerchantId == null), cancellationToken))
        {
            return Result<OrderDetailDto>.BadRequest("Carrier was not found.");
        }

        order.RecipientName = request.RecipientName.Trim();

        order.AltPhoneE164 = altPhone;

        order.RegionId = request.RegionId;

        order.City = request.City?.Trim();

        order.AddressLine = request.AddressLine.Trim();

        order.Landmark = request.Landmark?.Trim();

        order.Latitude = request.Latitude;

        order.Longitude = request.Longitude;

        order.ShippingFee = request.ShippingFee;

        order.DiscountAmount = request.DiscountAmount;

        order.TotalAmount = total;

        order.PaymentMethod = request.PaymentMethod;

        order.DepositAmount = request.DepositAmount;

        order.ExpectedDeliveryDate = request.ExpectedDeliveryDate;

        order.CarrierId = request.CarrierId;

        order.TrackingNo = request.TrackingNo?.Trim();

        order.Notes = request.Notes?.Trim();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result<OrderDetailDto>> UpdateItemsAsync(long id, UpdateOrderItemsRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result<OrderDetailDto>.NotFound("Order was not found.");
        }

        if (!EditableStatuses.Contains(order.StatusId))
        {
            return Result<OrderDetailDto>.BadRequest("This order can no longer be edited.");
        }

        var productIds = request.Items.Where(item => item.ProductId.HasValue).Select(item => item.ProductId!.Value).Distinct().ToList();

        if (productIds.Count > 0 && await unitOfWork.Repository<Product>().CountAsync(product => productIds.Contains(product.Id) && product.MerchantId == merchantId, cancellationToken) != productIds.Count)
        {
            return Result<OrderDetailDto>.BadRequest("One or more products were not found.");
        }

        var itemsTotal = request.Items.Sum(item => item.Quantity * item.UnitPrice);

        var total = itemsTotal + order.ShippingFee - order.DiscountAmount;

        if (total < 0)
        {
            return Result<OrderDetailDto>.BadRequest("Discount cannot exceed the order value.");
        }

        if (order.DepositAmount > total)
        {
            return Result<OrderDetailDto>.BadRequest("The deposit cannot exceed the new order total.");
        }

        var currentItems = await unitOfWork.Repository<OrderItem>().Query(false).Where(item => item.OrderId == id).ToListAsync(cancellationToken);

        unitOfWork.Repository<OrderItem>().RemoveRange(currentItems);

        await unitOfWork.Repository<OrderItem>().AddRangeAsync(request.Items.Select(item => new OrderItem
        {
            OrderId = id,

            ProductId = item.ProductId,

            ProductName = item.ProductName.Trim(),

            Variant = item.Variant?.Trim(),

            Quantity = item.Quantity,

            UnitPrice = item.UnitPrice
        }).ToList(), cancellationToken);

        order.ItemsTotal = itemsTotal;

        order.TotalAmount = total;

        order.StatusHistory.Add(new OrderStatusHistory
        {
            FromStatusId = order.StatusId,

            ToStatusId = order.StatusId,

            ChangedByType = ActorType.MerchantUser,

            ChangedByUserId = userAppService.UserId,

            Note = "Order items updated.",

            ChangedAt = DateTime.UtcNow
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result<OrderDetailDto>> ChangeStatusAsync(long id, ChangeOrderStatusRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result<OrderDetailDto>.NotFound("Order was not found.");
        }

        var changed = await ApplyStatusAsync(order, request.StatusId, request.Note, cancellationToken);

        if (!changed.Succeeded)
        {
            return CopyFailure(changed);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result<OrderDetailDto>> AddDeliveryAttemptAsync(long id, AddDeliveryAttemptRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result<OrderDetailDto>.NotFound("Order was not found.");
        }

        if (order.StatusId is not (OrderStatusType.Shipped or OrderStatusType.OutForDelivery or OrderStatusType.FailedDelivery))
        {
            return Result<OrderDetailDto>.BadRequest("Delivery attempts can only be recorded for shipped orders.");
        }

        var attempts = await unitOfWork.Repository<DeliveryAttempt>().CountAsync(item => item.OrderId == id, cancellationToken);

        await unitOfWork.Repository<DeliveryAttempt>().AddAsync(new DeliveryAttempt
        {
            OrderId = id,

            AttemptNo = (byte)(attempts + 1),

            AttemptedAt = request.AttemptedAt ?? DateTime.UtcNow,

            Result = request.Result,

            CarrierNote = request.CarrierNote?.Trim(),

            ProofImageUrl = request.ProofImageUrl?.Trim()
        }, cancellationToken);

        var target = request.Result switch
        {
            DeliveryAttemptResult.Delivered => OrderStatusType.Delivered,

            DeliveryAttemptResult.Refused => OrderStatusType.Refused,

            _ => OrderStatusType.FailedDelivery
        };

        if (target != order.StatusId)
        {
            var changed = await ApplyStatusAsync(order, target, request.CarrierNote, cancellationToken);

            if (!changed.Succeeded)
            {
                return CopyFailure(changed);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result.NotFound("Order was not found.");
        }

        if (!DeletableStatuses.Contains(order.StatusId) && !order.IsHistorical)
        {
            return Result.BadRequest("Only new or cancelled orders can be deleted.");
        }

        unitOfWork.Repository<Order>().Remove(order);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok("Order deleted.");
    }

    private async Task<Result> ApplyStatusAsync(Order order, OrderStatusType target, string? note, CancellationToken cancellationToken)
    {
        if (order.StatusId == target)
        {
            return Result.BadRequest("The order is already in this status.");
        }

        if (!Transitions.TryGetValue(order.StatusId, out var allowed) || !allowed.Contains(target))
        {
            return Result.BadRequest($"Cannot move the order from {order.StatusId} to {target}.");
        }

        var now = DateTime.UtcNow;

        var previous = order.StatusId;

        order.StatusId = target;

        order.StatusHistory.Add(new OrderStatusHistory
        {
            FromStatusId = previous,

            ToStatusId = target,

            ChangedByType = ActorType.MerchantUser,

            ChangedByUserId = userAppService.UserId,

            Note = note?.Trim(),

            ChangedAt = now
        });

        switch (target)
        {
            case OrderStatusType.Shipped:

            order.ShippedAt ??= now;

            break;

            case OrderStatusType.Delivered:

            order.DeliveredAt = now;

            await outcomes.ApplyFinalResultAsync(order, OrderFinalResult.Delivered, cancellationToken);

            break;

            case OrderStatusType.Refused:

            await outcomes.ApplyFinalResultAsync(order, OrderFinalResult.Refused, cancellationToken);

            if (!await unitOfWork.Repository<OrderPublicLink>().AnyAsync(link => link.OrderId == order.Id && link.Purpose == PublicLinkPurpose.RejectionSurvey && link.ExpiresAt > now, cancellationToken))
            {
                await unitOfWork.Repository<OrderPublicLink>().AddAsync(linkFactory.Create(order, PublicLinkPurpose.RejectionSurvey), cancellationToken);
            }

            break;

            case OrderStatusType.CancelledByMerchant:

            case OrderStatusType.CancelledByCustomer:

            await outcomes.ApplyFinalResultAsync(order, OrderFinalResult.Cancelled, cancellationToken);

            break;
        }

        return Result.Ok();
    }

    private static Result<OrderDetailDto> CopyFailure(Result failure)
    {
        IEnumerable<string> errors = failure.Errors.Count > 0 ? failure.Errors : new[] { failure.Message ?? "Bad request." };

        return failure.Status switch
        {
            ResultStatus.NotFound => Result<OrderDetailDto>.NotFound(failure.Message),

            ResultStatus.Conflict => Result<OrderDetailDto>.Conflict(failure.Message),

            ResultStatus.Forbidden => Result<OrderDetailDto>.Forbidden(failure.Message),

            _ => Result<OrderDetailDto>.BadRequest(errors)
        };
    }
}
