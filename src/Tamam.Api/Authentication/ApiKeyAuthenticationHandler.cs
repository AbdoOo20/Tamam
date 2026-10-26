using System.Security.Claims;

using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;

using Microsoft.Extensions.Options;

using Tamam.Api.Extensions;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.Services.Integrations;

using Tamam.Domain.Enums;

namespace Tamam.Api.Authentication;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,

    ILoggerFactory logger,

    UrlEncoder encoder,

    IApiKeyService apiKeyService) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyDefaults.HeaderName, out var values))
            {
                return AuthenticateResult.NoResult();
        }

        var merchant = await apiKeyService.AuthenticateAsync(values.ToString(), Context.RequestAborted);

        if (merchant is null)
        {
            return AuthenticateResult.Fail("Invalid API key.");
        }

        if (merchant.Status is MerchantStatus.Suspended or MerchantStatus.Closed)
        {
            return AuthenticateResult.Fail("The merchant account is not active.");
        }

        var identity = new ClaimsIdentity(
            [

            new Claim(AppClaimTypes.MerchantId, merchant.Id.ToString()),

            new Claim("role", ApiKeyDefaults.Role)

            ],

            Scheme.Name,

            "name",

            "role");

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        return Response.WriteResultAsync(Result.Unauthorized("A valid API key is required."), Context.RequestAborted);
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        return Response.WriteResultAsync(Result.Forbidden(), Context.RequestAborted);
    }
}
