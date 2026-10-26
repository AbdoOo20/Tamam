using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Billing;

using Tamam.Application.Services.Billing;

namespace Tamam.Api.Controllers;

[Authorize]
public class BillingController(IBillingService billingService) : ApiControllerBase
{
    [HttpGet("plans")]
    public async Task<ActionResult<Result<IReadOnlyList<PlanDto>>>> GetPlans(CancellationToken cancellationToken)
    {
        return Respond(await billingService.GetPlansAsync(cancellationToken));
    }

    [HttpGet("subscription")]
    public async Task<ActionResult<Result<SubscriptionDto>>> GetSubscription(CancellationToken cancellationToken)
    {
        return Respond(await billingService.GetSubscriptionAsync(cancellationToken));
    }

    [HttpGet("usage")]
    public async Task<ActionResult<Result<UsageSummaryDto>>> GetUsage([FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken cancellationToken)
    {
        return Respond(await billingService.GetUsageAsync(from, to, cancellationToken));
    }
}
