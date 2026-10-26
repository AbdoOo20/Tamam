using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Staff;

using Tamam.Application.Services.Staff;

using Tamam.Domain.Constants;

namespace Tamam.Api.Controllers;

[Authorize]
public class StaffController(IStaffService staffService) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<IReadOnlyList<StaffDto>>>> GetMyStaff(CancellationToken cancellationToken)
    {
        return Respond(await staffService.GetMyStaffAsync(cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<ActionResult<Result<StaffDto>>> Create(CreateStaffRequest request, CancellationToken cancellationToken)
    {
        return Respond(await staffService.CreateAsync(request, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<ActionResult<Result>> Delete(int id, CancellationToken cancellationToken)
    {
        return Respond(await staffService.DeleteAsync(id, cancellationToken));
    }
}
