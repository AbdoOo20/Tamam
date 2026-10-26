using System.Security.Claims;

using System.Text;

using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Options;

using Microsoft.IdentityModel.JsonWebTokens;

using Microsoft.IdentityModel.Tokens;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Auth;

using Tamam.Application.Services.Auth;

using Tamam.Application.Settings;

using Tamam.Domain.Constants;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Services.Auth;

public class AuthService(UserManager<User> userManager, IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtOptions) : IAuthService
{
    private readonly JwtSettings jwt = jwtOptions.Value;

    public async Task<Result<AuthResponse>> RegisterMerchantAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var country = await unitOfWork.Repository<Country>().GetByIdAsync(request.CountryCode.Trim().ToUpperInvariant(), cancellationToken);

        if (country is null || !country.IsActive)
        {
            return Result<AuthResponse>.Fail("Country is not supported.");
        }

        if (await userManager.FindByEmailAsync(request.Email) is not null)
        {
            return Result<AuthResponse>.Fail("Email is already registered.");
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var merchant = new Merchant
            {
                BusinessName = request.BusinessName,

                OwnerName = request.OwnerName,

                Phone = request.Phone,

                Email = request.Email,

                CountryCode = country.Id,

                CurrencyCode = country.CurrencyCode
            };

            await unitOfWork.Repository<Merchant>().AddAsync(merchant, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var user = new User
            {
                UserName = request.Email,

                Email = request.Email,

                EmailConfirmed = true,

                PhoneNumber = request.Phone,

                FullName = request.OwnerName,

                MerchantId = merchant.Id
            };

            var createResult = await userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);

                return Result<AuthResponse>.Fail(createResult.Errors.Select(error => error.Description));
            }

            await userManager.AddToRoleAsync(user, RoleNames.Owner);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return Result<AuthResponse>.Ok(await BuildResponseAsync(user));
        }

        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            throw;
        }
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        const string invalidCredentials = "Invalid email or password.";

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null || !user.IsActive)
        {
            return Result<AuthResponse>.Fail(invalidCredentials);
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return Result<AuthResponse>.Fail("Account is locked. Try again later.");
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            await userManager.AccessFailedAsync(user);

            return Result<AuthResponse>.Fail(invalidCredentials);
        }

        await userManager.ResetAccessFailedCountAsync(user);

        user.LastLoginAt = DateTime.UtcNow;

        await userManager.UpdateAsync(user);

        return Result<AuthResponse>.Ok(await BuildResponseAsync(user));
    }

    private async Task<AuthResponse> BuildResponseAsync(User user)
    {
        var roles = await userManager.GetRolesAsync(user);

        var expiresAt = DateTime.UtcNow.AddMinutes(jwt.ExpiryMinutes);

        var claims = new List<Claim>
        {
            new(AppClaimTypes.UserId, user.Id.ToString()),

            new("email", user.Email ?? string.Empty),

            new("name", user.FullName)
        };

        if (user.MerchantId is { } merchantId)
        {
            claims.Add(new Claim(AppClaimTypes.MerchantId, merchantId.ToString()));
        }

        claims.AddRange(roles.Select(role => new Claim("role", role)));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Issuer = jwt.Issuer,

            Audience = jwt.Audience,

            Expires = expiresAt,

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)), SecurityAlgorithms.HmacSha256)
        };

        var token = new JsonWebTokenHandler().CreateToken(descriptor);

        return new AuthResponse(token, expiresAt, user.Id, user.MerchantId, user.FullName, roles.FirstOrDefault() ?? string.Empty);
    }
}
