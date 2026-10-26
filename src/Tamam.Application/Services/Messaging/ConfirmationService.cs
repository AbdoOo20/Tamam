using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Messaging;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Messaging;

public class ConfirmationService(IUnitOfWork unitOfWork, IUserAppService userAppService, IMessageBuilder messageBuilder, PublicLinkFactory linkFactory) : IConfirmationService
{
    public async Task<ConfirmationDto> AttachNewConfirmationAsync(Order order, IEnumerable<OrderItem> items, Merchant merchant, byte attemptNo, CancellationToken cancellationToken = default)
    {
        var link = linkFactory.Create(order, PublicLinkPurpose.OrderConfirmation);

        var url = linkFactory.BuildUrl(link.Purpose, link.Token);

        var message = await messageBuilder.BuildOrderConfirmationAsync(order, items, merchant, url, cancellationToken);

        var request = new ConfirmationRequest
        {
            Order = order,

            AttemptNo = attemptNo,

            Channel = MessageChannel.WaMeLink,

            TemplateId = message.TemplateId,

            Status = ConfirmationStatus.Created
        };

        order.PublicLinks.Add(link);

        order.ConfirmationRequests.Add(request);

        return ToDto(order.Id, request, url, link.ExpiresAt, message);
    }

    public async Task<Result<ConfirmationDto>> GetCurrentAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == orderId && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result<ConfirmationDto>.NotFound("Order was not found.");
        }

        var request = await unitOfWork.Repository<ConfirmationRequest>().Query()
            .Where(item => item.OrderId == orderId)
            .OrderByDescending(item => item.AttemptNo)
            .FirstOrDefaultAsync(cancellationToken);

        var link = await unitOfWork.Repository<OrderPublicLink>().Query()
            .Where(item => item.OrderId == orderId && item.Purpose == PublicLinkPurpose.OrderConfirmation)
            .OrderByDescending(item => item.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (request is null || link is null)
        {
            return Result<ConfirmationDto>.NotFound("No confirmation has been prepared for this order.");
        }

        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(merchantId, cancellationToken);

        var items = await unitOfWork.Repository<OrderItem>().ListAsync(item => item.OrderId == orderId, cancellationToken);

        var url = linkFactory.BuildUrl(link.Purpose, link.Token);

        var message = await messageBuilder.BuildOrderConfirmationAsync(order, items, merchant!, url, cancellationToken);

        return Result<ConfirmationDto>.Ok(ToDto(orderId, request, url, link.ExpiresAt, message));
    }

    public async Task<Result<ConfirmationDto>> RegenerateAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == orderId && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result<ConfirmationDto>.NotFound("Order was not found.");
        }

        if (order.StatusId is not (OrderStatusType.New or OrderStatusType.AwaitingConfirmation))
        {
            return Result<ConfirmationDto>.BadRequest("A new confirmation can only be generated for orders that are not confirmed yet.");
        }

        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(merchantId, cancellationToken);

        var items = await unitOfWork.Repository<OrderItem>().ListAsync(item => item.OrderId == orderId, cancellationToken);

        var attempts = await unitOfWork.Repository<ConfirmationRequest>().CountAsync(item => item.OrderId == orderId, cancellationToken);

        var dto = await AttachNewConfirmationAsync(order, items, merchant!, (byte)(attempts + 1), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ConfirmationDto>.Created(dto);
    }

    public async Task<Result<ConfirmationDto>> MarkSentAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == orderId && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result<ConfirmationDto>.NotFound("Order was not found.");
        }

        var customer = await unitOfWork.Repository<Customer>().GetByIdAsync(order.CustomerId, cancellationToken);

        if (customer is { IsOptedOut: true })
        {
            return Result<ConfirmationDto>.BadRequest("This customer opted out of receiving messages.");
        }

        var request = await unitOfWork.Repository<ConfirmationRequest>().Query(asNoTracking: false)
            .Where(item => item.OrderId == orderId)
            .OrderByDescending(item => item.AttemptNo)
            .FirstOrDefaultAsync(cancellationToken);

        if (request is null)
        {
            return Result<ConfirmationDto>.NotFound("No confirmation has been prepared for this order.");
        }

        if (request.Status != ConfirmationStatus.Created)
        {
            return await GetCurrentAsync(orderId, cancellationToken);
        }

        var current = await GetCurrentAsync(orderId, cancellationToken);

        if (!current.Succeeded)
        {
            return current;
        }

        var now = DateTime.UtcNow;

        request.Status = ConfirmationStatus.Sent;

        request.SentAt = now;

        await unitOfWork.Repository<MessageLog>().AddAsync(new MessageLog
        {
            OrderId = orderId,

            ConfirmationRequest = request,

            Direction = MessageDirection.Outbound,

            Channel = MessageChannel.WaMeLink,

            ToPhoneE164 = order.RecipientPhoneE164,

            Body = current.Data!.MessageBody,

            DeliveryStatus = MessageDeliveryStatus.Sent
        }, cancellationToken);

        if (order.StatusId == OrderStatusType.New)
        {
            order.StatusId = OrderStatusType.AwaitingConfirmation;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                FromStatusId = OrderStatusType.New,

                ToStatusId = OrderStatusType.AwaitingConfirmation,

                ChangedByType = ActorType.MerchantUser,

                ChangedByUserId = userAppService.UserId,

                Note = "Confirmation message sent.",

                ChangedAt = now
            });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetCurrentAsync(orderId, cancellationToken);
    }

    private static ConfirmationDto ToDto(long orderId, ConfirmationRequest request, string url, DateTime expiresAt, BuiltMessage message) => new(
        orderId,

        request.AttemptNo,

        request.Status,

        url,

        expiresAt,

        message.Body,

        message.WhatsAppUrl,

        request.SentAt,

        request.RespondedAt,

        request.ProposedDeliveryDate,

        request.ResponseNote);
}
