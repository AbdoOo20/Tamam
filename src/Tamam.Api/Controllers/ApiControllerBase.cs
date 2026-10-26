using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

namespace Tamam.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected ObjectResult Respond(Result result) => StatusCode(result.StatusCode, result);
}
