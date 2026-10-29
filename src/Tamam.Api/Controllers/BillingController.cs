using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Billing;

using Tamam.Application.Services.Billing;

using Tamam.Domain.Constants;

namespace Tamam.Api.Controllers;

[Authorize]
public class BillingController(IBillingService billingService) : ApiControllerBase
{
    [AllowAnonymous]

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

    [HttpPost("subscription/change")]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<ActionResult<Result<SubscriptionDto>>> ChangePlan(ChangePlanRequest request, CancellationToken cancellationToken)
    {
        return Respond(await billingService.ChangePlanAsync(request, cancellationToken));
    }

    [HttpGet("usage")]
    public async Task<ActionResult<Result<UsageSummaryDto>>> GetUsage([FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken cancellationToken)
    {
        return Respond(await billingService.GetUsageAsync(from, to, cancellationToken));
    }
}
