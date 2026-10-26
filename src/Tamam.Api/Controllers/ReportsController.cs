using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Reports;

using Tamam.Application.Services.Reports;

namespace Tamam.Api.Controllers;

[Authorize]
public class ReportsController(IReportService reportService) : ApiControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<Result<DashboardDto>>> GetDashboard([FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken cancellationToken)
    {
        return Respond(await reportService.GetDashboardAsync(from, to, cancellationToken));
    }

    [HttpGet("quality")]
    public async Task<ActionResult<Result<IReadOnlyList<QualityMonthDto>>>> GetQuality([FromQuery] int months = 6, CancellationToken cancellationToken = default)
    {
        return Respond(await reportService.GetQualityAsync(months, cancellationToken));
    }
}
