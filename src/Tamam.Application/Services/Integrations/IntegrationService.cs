using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Integrations;

using Tamam.Application.DTOs.Orders;

using Tamam.Application.Services.Messaging;

using Tamam.Application.Services.Orders;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Integrations;

public class IntegrationService(
    IUnitOfWork unitOfWork,

    IUserAppService userAppService,

    IOrderCreationService orderCreation,

    IConfirmationService confirmations) : IIntegrationService
    {
        public async Task<Result<IntegrationOrderDto>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            var merchantId = userAppService.RequiredMerchantId;

            var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(merchantId, cancellationToken);

        if (merchant is null)
        {
            return Result<IntegrationOrderDto>.NotFound("Merchant was not found.");
        }

        if (merchant.Status is MerchantStatus.Suspended or MerchantStatus.Closed)
        {
            return Result<IntegrationOrderDto>.Forbidden("This merchant account is not active.");
        }

        var orderNo = request.OrderNo?.Trim();

        if (!string.IsNullOrEmpty(orderNo))
        {
            var existingId = await unitOfWork.Repository<Order>().Query()
                .Where(order => order.MerchantId == merchantId && order.OrderNo == orderNo)
                .Select(order => (long?)order.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingId is { } alreadyReceivedId)
            {
                return await BuildAsync(alreadyReceivedId, true, "Order was already received.", cancellationToken);
            }
        }

        var country = await unitOfWork.Repository<Country>().GetByIdAsync(merchant.CountryCode, cancellationToken);

        var created = await orderCreation.CreateAsync(request, merchant, country?.PhoneCode ?? "+", null, OrderSource.Api, false, cancellationToken);

        if (!created.Succeeded)
        {
            return Result<IntegrationOrderDto>.From(created);
        }

        var built = await BuildAsync(created.Data, false, "Order received.", cancellationToken);

        return built.Succeeded ? Result<IntegrationOrderDto>.Created(built.Data!, built.Message) : built;
    }

    public async Task<Result<IntegrationOrderDto>> GetOrderAsync(string orderNo, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var term = orderNo.Trim();

        var orderId = await unitOfWork.Repository<Order>().Query()
            .Where(order => order.MerchantId == merchantId && order.OrderNo == term)
            .Select(order => (long?)order.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (orderId is null)
        {
            return Result<IntegrationOrderDto>.NotFound("Order was not found.");
        }

        return await BuildAsync(orderId.Value, true, null, cancellationToken);
    }

    private async Task<Result<IntegrationOrderDto>> BuildAsync(long orderId, bool alreadyExisted, string? message, CancellationToken cancellationToken)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var row = await unitOfWork.Repository<Order>().Query()
            .Where(order => order.Id == orderId && order.MerchantId == merchantId)
            .Select(order => new
        {
            order.Id,

            order.OrderNo,

            order.StatusId,

            StatusName = order.Status.NameAr,

            order.TotalAmount,

            order.AmountToCollect,

            order.LatestRiskScore,

            order.LatestRiskLevel
        })
            .FirstOrDefaultAsync(cancellationToken);

        if (row is null)
        {
            return Result<IntegrationOrderDto>.NotFound("Order was not found.");
        }

        var action = await unitOfWork.Repository<OrderRiskAssessment>().Query()
            .Where(assessment => assessment.OrderId == orderId)
            .OrderByDescending(assessment => assessment.CreatedAt)
            .ThenByDescending(assessment => assessment.Id)
            .Select(assessment => (RecommendedActionType?)assessment.RecommendedAction)
            .FirstOrDefaultAsync(cancellationToken);

        var confirmation = (await confirmations.GetCurrentAsync(orderId, cancellationToken)).Data;

        var dto = new IntegrationOrderDto(
            row.Id,

            row.OrderNo,

            row.StatusId,

            row.StatusName,

            row.TotalAmount,

            row.AmountToCollect,

            row.LatestRiskScore,

            row.LatestRiskLevel,

            action,

            confirmation?.Link,

            confirmation?.WhatsAppUrl,

            confirmation?.MessageBody,

            alreadyExisted);

        return Result<IntegrationOrderDto>.Ok(dto, message);
    }
}
