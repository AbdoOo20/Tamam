using System.Security.Cryptography;

using System.Text;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Integrations;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Integrations;

public class ApiKeyService(IUnitOfWork unitOfWork, IUserAppService userAppService) : IApiKeyService
{
    private const string Prefix = "tmk_";

    public async Task<Result<ApiKeyStatusDto>> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(userAppService.RequiredMerchantId, cancellationToken);

        return merchant is null
            ? Result<ApiKeyStatusDto>.NotFound("Merchant was not found.")
            : Result<ApiKeyStatusDto>.Ok(new ApiKeyStatusDto(merchant.ApiKeyHash is not null));
    }

    public async Task<Result<ApiKeyDto>> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(userAppService.RequiredMerchantId, cancellationToken);

        if (merchant is null)
        {
            return Result<ApiKeyDto>.NotFound("Merchant was not found.");
        }

        var secret = Convert.ToHexString(RandomNumberGenerator.GetBytes(32)).ToLowerInvariant();

        merchant.ApiKeyHash = HashSecret(secret);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ApiKeyDto>.Ok(new ApiKeyDto($"{Prefix}{merchant.Id}_{secret}"), "Store this key now. It will not be shown again.");
    }

    public async Task<Result> RevokeAsync(CancellationToken cancellationToken = default)
    {
        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(userAppService.RequiredMerchantId, cancellationToken);

        if (merchant is null)
        {
            return Result.NotFound("Merchant was not found.");
        }

        merchant.ApiKeyHash = null;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok("API key revoked.");
    }

    public async Task<Merchant?> AuthenticateAsync(string? apiKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey) || !apiKey.StartsWith(Prefix, StringComparison.Ordinal))
        {
            return null;
        }

        var parts = apiKey[Prefix.Length..].Split('_', 2);

        if (parts.Length != 2 || !int.TryParse(parts[0], out var merchantId))
        {
            return null;
        }

        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(merchantId, cancellationToken);

        if (merchant?.ApiKeyHash is null)
        {
            return null;
        }

        return CryptographicOperations.FixedTimeEquals(HashSecret(parts[1]), merchant.ApiKeyHash) ? merchant : null;
    }

    private static byte[] HashSecret(string secret) => SHA256.HashData(Encoding.UTF8.GetBytes(secret));
}
