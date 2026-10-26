using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Admin;

using Tamam.Application.DTOs.Audit;

using Tamam.Application.DTOs.Billing;

using Tamam.Application.DTOs.DataRequests;

using Tamam.Application.Services.Admin;

using Tamam.Application.Services.Audit;

using Tamam.Application.Services.DataRequests;

using Tamam.Domain.Constants;

namespace Tamam.Api.Controllers;

[Authorize(Roles = RoleNames.PlatformAdmin)]
public class AdminController(
    IAdminMerchantService merchantService,

    IAdminCatalogService catalogService,

    IDataRequestService dataRequestService,

    IAuditLogService auditLogService) : ApiControllerBase
    {
        [HttpGet("overview")]
        public async Task<ActionResult<Result<AdminOverviewDto>>> GetOverview(CancellationToken cancellationToken)
        {
            return Respond(await merchantService.GetOverviewAsync(cancellationToken));
    }

    [HttpGet("merchants")]
    public async Task<ActionResult<Result<PagedResult<AdminMerchantListItemDto>>>> GetMerchants([FromQuery] AdminMerchantQuery query, CancellationToken cancellationToken)
    {
        return Respond(await merchantService.GetAllAsync(query, cancellationToken));
    }

    [HttpGet("merchants/{id:int}")]
    public async Task<ActionResult<Result<AdminMerchantDetailDto>>> GetMerchant(int id, CancellationToken cancellationToken)
    {
        return Respond(await merchantService.GetByIdAsync(id, cancellationToken));
    }

    [HttpPut("merchants/{id:int}/status")]
    public async Task<ActionResult<Result<AdminMerchantDetailDto>>> UpdateMerchantStatus(int id, UpdateMerchantStatusRequest request, CancellationToken cancellationToken)
    {
        return Respond(await merchantService.UpdateStatusAsync(id, request, cancellationToken));
    }

    [HttpPut("merchants/{id:int}/subscription")]
    public async Task<ActionResult<Result<SubscriptionDto>>> ChangeMerchantPlan(int id, ChangePlanRequest request, CancellationToken cancellationToken)
    {
        return Respond(await merchantService.ChangePlanAsync(id, request, cancellationToken));
    }

    [HttpGet("plans")]
    public async Task<ActionResult<Result<IReadOnlyList<AdminPlanDto>>>> GetPlans(CancellationToken cancellationToken)
    {
        return Respond(await catalogService.GetPlansAsync(cancellationToken));
    }

    [HttpPost("plans")]
    public async Task<ActionResult<Result<AdminPlanDto>>> CreatePlan(AdminPlanRequest request, CancellationToken cancellationToken)
    {
        return Respond(await catalogService.CreatePlanAsync(request, cancellationToken));
    }

    [HttpPut("plans/{id:int}")]
    public async Task<ActionResult<Result<AdminPlanDto>>> UpdatePlan(short id, AdminPlanRequest request, CancellationToken cancellationToken)
    {
        return Respond(await catalogService.UpdatePlanAsync(id, request, cancellationToken));
    }

    [HttpGet("risk-rules")]
    public async Task<ActionResult<Result<IReadOnlyList<AdminRiskRuleDto>>>> GetRiskRules(CancellationToken cancellationToken)
    {
        return Respond(await catalogService.GetRiskRulesAsync(cancellationToken));
    }

    [HttpPut("risk-rules/{id:int}")]
    public async Task<ActionResult<Result<AdminRiskRuleDto>>> UpdateRiskRule(short id, UpdateRiskRuleRequest request, CancellationToken cancellationToken)
    {
        return Respond(await catalogService.UpdateRiskRuleAsync(id, request, cancellationToken));
    }

    [HttpGet("data-requests")]
    public async Task<ActionResult<Result<PagedResult<DataRequestDto>>>> GetDataRequests([FromQuery] DataRequestQuery query, CancellationToken cancellationToken)
    {
        return Respond(await dataRequestService.GetAllAsync(query, cancellationToken));
    }

    [HttpPost("data-requests/{id:int}/process")]
    public async Task<ActionResult<Result<DataRequestDto>>> ProcessDataRequest(int id, ProcessDataRequestRequest request, CancellationToken cancellationToken)
    {
        return Respond(await dataRequestService.ProcessAsync(id, request, cancellationToken));
    }

    [HttpGet("data-requests/{id:int}/export")]
    public async Task<ActionResult<Result<CustomerDataExportDto>>> ExportDataRequest(int id, CancellationToken cancellationToken)
    {
        return Respond(await dataRequestService.ExportAsync(id, cancellationToken));
    }

    [HttpGet("audit-logs")]
    public async Task<ActionResult<Result<PagedResult<AuditLogDto>>>> GetAuditLogs([FromQuery] AuditLogQuery query, CancellationToken cancellationToken)
    {
        return Respond(await auditLogService.GetForAdminAsync(query, cancellationToken));
    }

    [HttpPost("quality/recalculate")]
    public async Task<ActionResult<Result<int>>> RecalculateQuality(RecalculateQualityRequest request, CancellationToken cancellationToken)
    {
        return Respond(await merchantService.RecalculateQualityAsync(request, cancellationToken));
    }
}
