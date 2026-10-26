using System.Text;

using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;

using Tamam.Api.Authentication;

using Tamam.Application.Common;

using Tamam.Application.Settings;

namespace Tamam.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,

                ValidAudience = jwtSettings.Audience,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                ValidateLifetime = true,

                ClockSkew = TimeSpan.FromMinutes(1),

                NameClaimType = "name",

                RoleClaimType = "role"
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();

                    await context.Response.WriteResultAsync(Result.Unauthorized(), context.HttpContext.RequestAborted);
                },

                OnForbidden = context => context.Response.WriteResultAsync(Result.Forbidden(), context.HttpContext.RequestAborted)
            };
        })
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyDefaults.Scheme, _ => { });

        return services;
    }
}
