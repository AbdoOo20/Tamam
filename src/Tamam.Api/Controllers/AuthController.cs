using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.RateLimiting;

using Tamam.Api.RateLimiting;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Auth;

using Tamam.Application.Services.Auth;

namespace Tamam.Api.Controllers;

public class AuthController(IAuthService authService, IUserAppService userAppService) : ApiControllerBase
{
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Auth)]
    [HttpPost("register")]
    public async Task<ActionResult<Result<AuthResponse>>> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        return Respond(await authService.RegisterMerchantAsync(request, cancellationToken));
    }

    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Auth)]
    [HttpPost("login")]
    public async Task<ActionResult<Result<AuthResponse>>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        return Respond(await authService.LoginAsync(request, cancellationToken));
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<Result<CurrentUserDto>> Me()
    {
        return Respond(Result<CurrentUserDto>.Ok(new CurrentUserDto(userAppService.UserId, userAppService.MerchantId, userAppService.Role)));
    }
}
