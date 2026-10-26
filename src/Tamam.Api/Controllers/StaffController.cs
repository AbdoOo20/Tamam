using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Staff;

using Tamam.Domain.Constants;

namespace Tamam.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class StaffController(IStaffService staffService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StaffDto>>> GetMyStaff(CancellationToken cancellationToken)
    {
        return Ok(await staffService.GetMyStaffAsync(cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<ActionResult<StaffDto>> Create(CreateStaffRequest request, CancellationToken cancellationToken)
    {
        var result = await staffService.CreateAsync(request, cancellationToken);

        return result.Succeeded ? Ok(result.Value) : BadRequest(new { errors = result.Errors });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await staffService.DeleteAsync(id, cancellationToken);

        return result.Succeeded ? NoContent() : BadRequest(new { errors = result.Errors });
    }
}
