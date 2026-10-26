using Tamam.Application.Common;

using Tamam.Application.DTOs.Billing;

namespace Tamam.Application.Services.Billing;

public interface IBillingService
{
    Task<Result<IReadOnlyList<PlanDto>>> GetPlansAsync(CancellationToken cancellationToken = default);

    Task<Result<SubscriptionDto>> GetSubscriptionAsync(CancellationToken cancellationToken = default);

    Task<Result<UsageSummaryDto>> GetUsageAsync(DateTime? from, DateTime? to, CancellationToken cancellationToken = default);
}
