using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Integrations;

using Tamam.Application.DTOs.Merchants;

using Tamam.Application.Services.Integrations;

using Tamam.Application.Services.Merchants;

using Tamam.Domain.Constants;

namespace Tamam.Api.Controllers;

[Authorize]
public class MerchantController(IMerchantService merchantService, IApiKeyService apiKeyService) : ApiControllerBase
{
    [HttpGet("profile")]
    public async Task<ActionResult<Result<MerchantProfileDto>>> GetProfile(CancellationToken cancellationToken)
    {
        return Respond(await merchantService.GetProfileAsync(cancellationToken));
    }

    [HttpPut("profile")]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<ActionResult<Result<MerchantProfileDto>>> UpdateProfile(UpdateMerchantProfileRequest request, CancellationToken cancellationToken)
    {
        return Respond(await merchantService.UpdateProfileAsync(request, cancellationToken));
    }

    [HttpGet("consent")]
    public async Task<ActionResult<Result<ConsentDto>>> GetConsent(CancellationToken cancellationToken)
    {
        return Respond(await merchantService.GetConsentAsync(cancellationToken));
    }

    [HttpPost("consent")]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<ActionResult<Result<ConsentDto>>> AcceptConsent(AcceptConsentRequest request, CancellationToken cancellationToken)
    {
        return Respond(await merchantService.AcceptConsentAsync(request, cancellationToken));
    }

    [HttpGet("api-key")]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<ActionResult<Result<ApiKeyStatusDto>>> GetApiKeyStatus(CancellationToken cancellationToken)
    {
        return Respond(await apiKeyService.GetStatusAsync(cancellationToken));
    }

    [HttpPost("api-key")]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<ActionResult<Result<ApiKeyDto>>> GenerateApiKey(CancellationToken cancellationToken)
    {
        return Respond(await apiKeyService.GenerateAsync(cancellationToken));
    }

    [HttpDelete("api-key")]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<ActionResult<Result>> RevokeApiKey(CancellationToken cancellationToken)
    {
        return Respond(await apiKeyService.RevokeAsync(cancellationToken));
    }
}
