using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Lookups;

using Tamam.Application.Services.Lookups;

namespace Tamam.Api.Controllers;

[Authorize]
public class LookupsController(ILookupService lookupService) : ApiControllerBase
{
    [HttpGet("regions")]
    public async Task<ActionResult<Result<IReadOnlyList<RegionDto>>>> GetRegions([FromQuery] string countryCode, CancellationToken cancellationToken)
    {
        return Respond(await lookupService.GetRegionsAsync(countryCode, cancellationToken));
    }

    [HttpGet("order-statuses")]
    public async Task<ActionResult<Result<IReadOnlyList<OrderStatusDto>>>> GetOrderStatuses(CancellationToken cancellationToken)
    {
        return Respond(await lookupService.GetOrderStatusesAsync(cancellationToken));
    }

    [HttpGet("rejection-reasons")]
    public async Task<ActionResult<Result<IReadOnlyList<RejectionReasonDto>>>> GetRejectionReasons(CancellationToken cancellationToken)
    {
        return Respond(await lookupService.GetRejectionReasonsAsync(cancellationToken));
    }

    [HttpGet("risk-rules")]
    public async Task<ActionResult<Result<IReadOnlyList<RiskRuleDto>>>> GetRiskRules(CancellationToken cancellationToken)
    {
        return Respond(await lookupService.GetRiskRulesAsync(cancellationToken));
    }

    [HttpGet("enums")]
    public ActionResult<Result<IReadOnlyDictionary<string, IReadOnlyList<EnumItemDto>>>> GetEnums()
    {
        return Respond(lookupService.GetEnums());
    }
}
