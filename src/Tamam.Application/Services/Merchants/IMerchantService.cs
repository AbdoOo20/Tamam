using Tamam.Application.Common;

using Tamam.Application.DTOs.Merchants;

namespace Tamam.Application.Services.Merchants;

public interface IMerchantService
{
    Task<Result<MerchantProfileDto>> GetProfileAsync(CancellationToken cancellationToken = default);

    Task<Result<MerchantProfileDto>> UpdateProfileAsync(UpdateMerchantProfileRequest request, CancellationToken cancellationToken = default);

    Task<Result<ConsentDto>> GetConsentAsync(CancellationToken cancellationToken = default);

    Task<Result<ConsentDto>> AcceptConsentAsync(AcceptConsentRequest request, CancellationToken cancellationToken = default);
}
