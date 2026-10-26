using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Rejections;

using Tamam.Application.Services.Rejections;

namespace Tamam.Api.Controllers;

[Authorize]
public class RejectionsController(IRejectionService rejectionService) : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Result<RejectionReportDto>>> Report(ReportRejectionRequest request, CancellationToken cancellationToken)
    {
        return Respond(await rejectionService.ReportAsync(request, cancellationToken));
    }
}
