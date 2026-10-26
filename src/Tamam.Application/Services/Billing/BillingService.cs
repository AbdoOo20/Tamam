using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Billing;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Billing;

public class BillingService(IUnitOfWork unitOfWork, IUserAppService userAppService) : IBillingService
{
    private const string TrialPlanCode = "TRIAL";

    public async Task<Result<IReadOnlyList<PlanDto>>> GetPlansAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<PlanDto> plans = await unitOfWork.Repository<Plan>().Query()
            .Where(plan => plan.IsActive)
            .OrderBy(plan => plan.MonthlyFee)
            .Select(plan => new PlanDto(plan.Id, plan.Code, plan.NameAr, plan.MonthlyFee, plan.PricePerCheckedOrder, plan.IncludedOrders, plan.CurrencyCode))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<PlanDto>>.Ok(plans);
    }

    public Task<Result<SubscriptionDto>> GetSubscriptionAsync(CancellationToken cancellationToken = default)
    {
        return LoadSubscriptionAsync(userAppService.RequiredMerchantId, cancellationToken);
    }

    public Task<Result<SubscriptionDto>> ChangePlanAsync(ChangePlanRequest request, CancellationToken cancellationToken = default)
    {
        return ChangePlanForMerchantAsync(userAppService.RequiredMerchantId, request, cancellationToken);
    }

    public async Task<Result<SubscriptionDto>> ChangePlanForMerchantAsync(int merchantId, ChangePlanRequest request, CancellationToken cancellationToken = default)
    {
        var code = request.PlanCode.Trim().ToUpperInvariant();

        if (code == TrialPlanCode)
        {
            return Result<SubscriptionDto>.BadRequest("The trial plan cannot be selected.");
        }

        var plan = await unitOfWork.Repository<Plan>().FirstOrDefaultAsync(item => item.Code == code && item.IsActive, cancellationToken);

        if (plan is null)
        {
            return Result<SubscriptionDto>.BadRequest("Plan was not found.");
        }

        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(merchantId, cancellationToken);

        if (merchant is null)
        {
            return Result<SubscriptionDto>.NotFound("Merchant was not found.");
        }

        var current = await unitOfWork.Repository<MerchantSubscription>().Query(false)
            .Where(subscription => subscription.MerchantId == merchantId && subscription.Status != SubscriptionStatus.Cancelled)
            .OrderByDescending(subscription => subscription.StartedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (current is not null && current.PlanId == plan.Id)
        {
            return Result<SubscriptionDto>.BadRequest("The merchant is already subscribed to this plan.");
        }

        var now = DateTime.UtcNow;

        if (current is not null)
        {
            current.Status = SubscriptionStatus.Cancelled;

            current.EndsAt = now;
        }

        await unitOfWork.Repository<MerchantSubscription>().AddAsync(new MerchantSubscription
        {
            MerchantId = merchantId,

            PlanId = plan.Id,

            Status = SubscriptionStatus.Active,

            StartedAt = now
        }, cancellationToken);

        if (merchant.Status == MerchantStatus.Trial)
        {
            merchant.Status = MerchantStatus.Active;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await LoadSubscriptionAsync(merchantId, cancellationToken);
    }

    public async Task<Result<UsageSummaryDto>> GetUsageAsync(DateTime? from, DateTime? to, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var end = (to ?? DateTime.UtcNow).Date.AddDays(1);

        var start = (from ?? end.AddDays(-31)).Date;

        var rows = await unitOfWork.Repository<UsageRecord>().Query()
            .Where(usage => usage.MerchantId == merchantId && usage.OccurredAt >= start && usage.OccurredAt < end)
            .GroupBy(usage => usage.Type)
            .Select(group => new { Type = group.Key, Quantity = group.Sum(usage => usage.Quantity), Cost = group.Sum(usage => usage.TotalCost) })
            .ToListAsync(cancellationToken);

        IReadOnlyList<UsageLineDto> lines = rows.Select(row => new UsageLineDto(row.Type, row.Quantity, row.Cost)).ToList();

        return Result<UsageSummaryDto>.Ok(new UsageSummaryDto(start, end.AddTicks(-1), lines, lines.Sum(line => line.TotalCost)));
    }

    private async Task<Result<SubscriptionDto>> LoadSubscriptionAsync(int merchantId, CancellationToken cancellationToken)
    {
        var row = await unitOfWork.Repository<MerchantSubscription>().Query()
            .Where(subscription => subscription.MerchantId == merchantId)
            .OrderByDescending(subscription => subscription.StartedAt)
            .Select(subscription => new
        {
            subscription.Id,

            subscription.Status,

            subscription.StartedAt,

            subscription.TrialEndsAt,

            subscription.EndsAt,

            Plan = new PlanDto(subscription.Plan.Id, subscription.Plan.Code, subscription.Plan.NameAr, subscription.Plan.MonthlyFee, subscription.Plan.PricePerCheckedOrder, subscription.Plan.IncludedOrders, subscription.Plan.CurrencyCode)
        })
            .FirstOrDefaultAsync(cancellationToken);

        if (row is null)
        {
            return Result<SubscriptionDto>.NotFound("No subscription was found.");
        }

        var now = DateTime.UtcNow;

        var periodStart = row.Status == SubscriptionStatus.Trial ? row.StartedAt : new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var used = await unitOfWork.Repository<UsageRecord>().Query()
            .Where(usage => usage.MerchantId == merchantId && usage.Type == UsageType.OrderCheck && usage.OccurredAt >= periodStart)
            .SumAsync(usage => (int?)usage.Quantity, cancellationToken) ?? 0;

        return Result<SubscriptionDto>.Ok(new SubscriptionDto(row.Id, row.Plan, row.Status, row.StartedAt, row.TrialEndsAt, row.EndsAt, used, Math.Max(0, row.Plan.IncludedOrders - used)));
    }
}
