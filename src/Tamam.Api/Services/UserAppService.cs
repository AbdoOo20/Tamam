using System.Security.Claims;

using Tamam.Application.Abstractions;

using Tamam.Application.Exceptions;

namespace Tamam.Api.Services;

public class UserAppService(IHttpContextAccessor httpContextAccessor) : IUserAppService
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

    public int? UserId => ReadInt(AppClaimTypes.UserId) ?? ReadInt(ClaimTypes.NameIdentifier);

    public int? MerchantId => ReadInt(AppClaimTypes.MerchantId);

    public string? Role => Principal?.FindFirst(ClaimTypes.Role)?.Value ?? Principal?.FindFirst("role")?.Value;

    public string? IpAddress => httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public int RequiredUserId => UserId ?? throw new UnauthorizedAccessException("The current user is not authenticated.");

    public int RequiredMerchantId => MerchantId ?? throw new ForbiddenException("The current user does not belong to a merchant.");

    private int? ReadInt(string claimType)
    {
        var value = Principal?.FindFirst(claimType)?.Value;

        return int.TryParse(value, out var result) ? result : null;
    }
}
