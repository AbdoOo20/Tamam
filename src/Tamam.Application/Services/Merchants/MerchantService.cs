using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Merchants;

using Tamam.Application.Mappings;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Merchants;

public class MerchantService(IUnitOfWork unitOfWork, IUserAppService userAppService) : IMerchantService
{
    public async Task<Result<MerchantProfileDto>> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(userAppService.RequiredMerchantId, cancellationToken);

        return merchant is null ? Result<MerchantProfileDto>.NotFound("Merchant was not found.") : Result<MerchantProfileDto>.Ok(merchant.ToDto());
    }

    public async Task<Result<MerchantProfileDto>> UpdateProfileAsync(UpdateMerchantProfileRequest request, CancellationToken cancellationToken = default)
    {
        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(userAppService.RequiredMerchantId, cancellationToken);

        if (merchant is null)
        {
            return Result<MerchantProfileDto>.NotFound("Merchant was not found.");
        }

        merchant.BusinessName = request.BusinessName.Trim();

        merchant.OwnerName = request.OwnerName.Trim();

        merchant.Phone = request.Phone.Trim();

        merchant.Email = request.Email?.Trim();

        merchant.DefaultLanguage = request.DefaultLanguage.Trim();

        merchant.StoreUrl = request.StoreUrl?.Trim();

        merchant.InstagramHandle = request.InstagramHandle?.Trim();

        merchant.ShadowMode = request.ShadowMode;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<MerchantProfileDto>.Ok(merchant.ToDto(), "Profile updated.");
    }

    public async Task<Result<ConsentDto>> GetConsentAsync(CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var consent = await unitOfWork.Repository<MerchantConsent>().Query()
            .Where(item => item.MerchantId == merchantId)
            .OrderByDescending(item => item.AcceptedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return consent is null ? Result<ConsentDto>.NotFound("No consent has been recorded yet.") : Result<ConsentDto>.Ok(consent.ToDto());
    }

    public async Task<Result<ConsentDto>> AcceptConsentAsync(AcceptConsentRequest request, CancellationToken cancellationToken = default)
    {
        var consent = new MerchantConsent
        {
            MerchantId = userAppService.RequiredMerchantId,

            TermsVersion = request.TermsVersion.Trim(),

            ShareOutcomesToNetwork = request.ShareOutcomesToNetwork,

            AcceptedByUserId = userAppService.UserId,

            AcceptedFromIp = userAppService.IpAddress,

            AcceptedAt = DateTime.UtcNow
        };

        await unitOfWork.Repository<MerchantConsent>().AddAsync(consent, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ConsentDto>.Created(consent.ToDto(), "Consent recorded.");
    }
}
