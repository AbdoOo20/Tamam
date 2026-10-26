using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Audit;

using Tamam.Application.Services.Audit;

using Tamam.Domain.Constants;

namespace Tamam.Api.Controllers;

[Authorize(Roles = RoleNames.Owner)]
public class AuditLogsController(IAuditLogService auditLogService) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<PagedResult<AuditLogDto>>>> GetAll([FromQuery] AuditLogQuery query, CancellationToken cancellationToken)
    {
        return Respond(await auditLogService.GetForMerchantAsync(query, cancellationToken));
    }
}
