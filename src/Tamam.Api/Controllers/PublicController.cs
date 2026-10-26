using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.RateLimiting;

using Tamam.Api.RateLimiting;

using Tamam.Application.Common;

using Tamam.Application.DTOs.DataRequests;

using Tamam.Application.DTOs.Lookups;

using Tamam.Application.DTOs.Public;

using Tamam.Application.Services.DataRequests;

using Tamam.Application.Services.Lookups;

using Tamam.Application.Services.Public;

namespace Tamam.Api.Controllers;

[AllowAnonymous]
[EnableRateLimiting(RateLimitPolicies.Public)]
public class PublicController(IPublicOrderService publicOrderService, ILookupService lookupService, IDataRequestService dataRequestService) : ApiControllerBase
{
    [HttpGet("links/{token}")]
    public async Task<ActionResult<Result<PublicOrderDto>>> Get(string token, CancellationToken cancellationToken)
    {
        return Respond(await publicOrderService.GetAsync(token, cancellationToken));
    }

    [HttpPost("links/{token}/confirm")]
    public async Task<ActionResult<Result<PublicOrderDto>>> Confirm(string token, PublicConfirmRequest request, CancellationToken cancellationToken)
    {
        return Respond(await publicOrderService.ConfirmAsync(token, request, cancellationToken));
    }

    [HttpPost("links/{token}/reschedule")]
    public async Task<ActionResult<Result<PublicOrderDto>>> Reschedule(string token, PublicRescheduleRequest request, CancellationToken cancellationToken)
    {
        return Respond(await publicOrderService.RescheduleAsync(token, request, cancellationToken));
    }

    [HttpPost("links/{token}/cancel")]
    public async Task<ActionResult<Result<PublicOrderDto>>> Cancel(string token, PublicCancelRequest request, CancellationToken cancellationToken)
    {
        return Respond(await publicOrderService.CancelAsync(token, request, cancellationToken));
    }

    [HttpPost("links/{token}/survey")]
    public async Task<ActionResult<Result<PublicOrderDto>>> SubmitSurvey(string token, PublicSurveyRequest request, CancellationToken cancellationToken)
    {
        return Respond(await publicOrderService.SubmitSurveyAsync(token, request, cancellationToken));
    }

    [HttpPost("links/{token}/opt-out")]
    public async Task<ActionResult<Result>> OptOut(string token, CancellationToken cancellationToken)
    {
        return Respond(await publicOrderService.OptOutAsync(token, cancellationToken));
    }

    [HttpGet("rejection-reasons")]
    public async Task<ActionResult<Result<IReadOnlyList<RejectionReasonDto>>>> GetRejectionReasons(CancellationToken cancellationToken)
    {
        return Respond(await lookupService.GetRejectionReasonsAsync(cancellationToken));
    }

    [HttpPost("links/{token}/data-request")]
    public async Task<ActionResult<Result>> SubmitDataRequest(string token, CreateDataRequestRequest request, CancellationToken cancellationToken)
    {
        return Respond(await dataRequestService.SubmitFromLinkAsync(token, request, cancellationToken));
    }
}
