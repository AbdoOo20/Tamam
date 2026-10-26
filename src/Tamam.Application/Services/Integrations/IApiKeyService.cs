using Tamam.Application.Common;

using Tamam.Application.DTOs.Integrations;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Integrations;

public interface IApiKeyService
{
    Task<Result<ApiKeyStatusDto>> GetStatusAsync(CancellationToken cancellationToken = default);

    Task<Result<ApiKeyDto>> GenerateAsync(CancellationToken cancellationToken = default);

    Task<Result> RevokeAsync(CancellationToken cancellationToken = default);

    Task<Merchant?> AuthenticateAsync(string? apiKey, CancellationToken cancellationToken = default);
}
