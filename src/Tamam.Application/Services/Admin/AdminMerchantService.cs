using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Admin;

using Tamam.Application.DTOs.Billing;

using Tamam.Application.Services.Billing;

using Tamam.Application.Services.Reports;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Admin;

public class AdminMerchantService(IUnitOfWork unitOfWork, IBillingService billingService, IQualityStatService qualityStats) : IAdminMerchantService
{
    public async Task<Result<AdminOverviewDto>> GetOverviewAsync(CancellationToken cancellationToken = default)
    {
        var merchants = unitOfWork.Repository<Merchant>().Query();

        var merchantCount = await merchants.CountAsync(cancellationToken);

        var active = await merchants.CountAsync(merchant => merchant.Status == MerchantStatus.Active, cancellationToken);

        var trial = await merchants.CountAsync(merchant => merchant.Status == MerchantStatus.Trial, cancellationToken);

        var suspended = await merchants.CountAsync(merchant => merchant.Status == MerchantStatus.Suspended, cancellationToken);

        var totalOrders = await unitOfWork.Repository<Order>().Query().CountAsync(cancellationToken);

        var since = DateTime.UtcNow.AddDays(-30);

        var recent = unitOfWork.Repository<Order>().Query().Where(order => !order.IsHistorical && order.PlacedAt >= since);

        var recentCount = await recent.CountAsync(cancellationToken);

        var delivered = await recent.CountAsync(order => order.StatusId == OrderStatusType.Delivered, cancellationToken);

        var refused = await recent.CountAsync(order => order.StatusId == OrderStatusType.Refused || order.StatusId == OrderStatusType.Returned, cancellationToken);

        var openRequests = await unitOfWork.Repository<CustomerDataRequest>().CountAsync(request => request.Status == DataRequestStatus.Open, cancellationToken);

        var completed = delivered + refused;

        var rate = completed == 0 ? 0m : Math.Round(refused * 100m / completed, 1);

        return Result<AdminOverviewDto>.Ok(new AdminOverviewDto(merchantCount, active, trial, suspended, totalOrders, recentCount, delivered, refused, rate, openRequests));
    }

    public async Task<Result<PagedResult<AdminMerchantListItemDto>>> GetAllAsync(AdminMerchantQuery query, CancellationToken cancellationToken = default)
    {
        var merchants = unitOfWork.Repository<Merchant>().Query();

        if (query.Status is { } status)
        {
            merchants = merchants.Where(merchant => merchant.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();

            merchants = merchants.Where(merchant => merchant.BusinessName.Contains(term) || merchant.OwnerName.Contains(term) || merchant.Phone.Contains(term) || (merchant.Email != null && merchant.Email.Contains(term)));
        }

        var page = await merchants
            .OrderByDescending(merchant => merchant.CreatedAt)
            .Select(merchant => new AdminMerchantListItemDto(
            merchant.Id,

            merchant.BusinessName,

            merchant.OwnerName,

            merchant.Phone,

            merchant.Email,

            merchant.CountryCode,

            merchant.Status,

            merchant.ShadowMode,

            merchant.Orders.Count,

            merchant.CreatedAt))
                .ToPagedResultAsync(query.Page, query.PageSize, cancellationToken);

        return Result<PagedResult<AdminMerchantListItemDto>>.Ok(page);
    }

    public async Task<Result<AdminMerchantDetailDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var row = await unitOfWork.Repository<Merchant>().Query()
            .Where(merchant => merchant.Id == id)
            .Select(merchant => new
        {
            merchant.Id,

            merchant.BusinessName,

            merchant.OwnerName,

            merchant.Phone,

            merchant.Email,

            merchant.CountryCode,

            merchant.CurrencyCode,

            merchant.StoreUrl,

            merchant.Status,

            merchant.ShadowMode,

            HasApiKey = merchant.ApiKeyHash != null,

            merchant.CreatedAt
        })
            .FirstOrDefaultAsync(cancellationToken);

        if (row is null)
        {
            return Result<AdminMerchantDetailDto>.NotFound("Merchant was not found.");
        }

        var subscription = await unitOfWork.Repository<MerchantSubscription>().Query()
            .Where(item => item.MerchantId == id)
            .OrderByDescending(item => item.StartedAt)
            .Select(item => new { item.Plan.Code, item.Status })
            .FirstOrDefaultAsync(cancellationToken);

        var users = await unitOfWork.Repository<User>().Query().CountAsync(user => user.MerchantId == id, cancellationToken);

        var orders = unitOfWork.Repository<Order>().Query().Where(order => order.MerchantId == id);

        var ordersCount = await orders.CountAsync(cancellationToken);

        var since = DateTime.UtcNow.AddDays(-30);

        var recent = orders.Where(order => !order.IsHistorical && order.PlacedAt >= since);

        var recentCount = await recent.CountAsync(cancellationToken);

        var refused = await recent.CountAsync(order => order.StatusId == OrderStatusType.Refused || order.StatusId == OrderStatusType.Returned, cancellationToken);

        var detail = new AdminMerchantDetailDto(
            row.Id,

            row.BusinessName,

            row.OwnerName,

            row.Phone,

            row.Email,

            row.CountryCode,

            row.CurrencyCode,

            row.StoreUrl,

            row.Status,

            row.ShadowMode,

            row.HasApiKey,

            subscription?.Code,

            subscription?.Status,

            users,

            ordersCount,

            recentCount,

            refused,

            row.CreatedAt);

        return Result<AdminMerchantDetailDto>.Ok(detail);
    }

    public async Task<Result<AdminMerchantDetailDto>> UpdateStatusAsync(int id, UpdateMerchantStatusRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Status == MerchantStatus.Trial)
        {
            return Result<AdminMerchantDetailDto>.BadRequest("A merchant cannot be moved back to the trial status.");
        }

        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(id, cancellationToken);

        if (merchant is null)
        {
            return Result<AdminMerchantDetailDto>.NotFound("Merchant was not found.");
        }

        merchant.Status = request.Status;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public Task<Result<SubscriptionDto>> ChangePlanAsync(int id, ChangePlanRequest request, CancellationToken cancellationToken = default)
    {
        return billingService.ChangePlanForMerchantAsync(id, request, cancellationToken);
    }

    public async Task<Result<int>> RecalculateQualityAsync(RecalculateQualityRequest request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var merchantsUpdated = await qualityStats.RecalculateAsync(request.Year ?? now.Year, request.Month ?? now.Month, cancellationToken);

        return Result<int>.Ok(merchantsUpdated, "Quality snapshots were recalculated.");
    }
}
