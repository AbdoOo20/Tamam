using Tamam.Application.DTOs.Merchants;

using Tamam.Domain.Entities;

namespace Tamam.Application.Mappings;

public static class MerchantMappings
{
    public static MerchantProfileDto ToDto(this Merchant merchant) => new(
        merchant.Id,

        merchant.BusinessName,

        merchant.OwnerName,

        merchant.Phone,

        merchant.Email,

        merchant.CountryCode,

        merchant.CurrencyCode,

        merchant.DefaultLanguage,

        merchant.StoreUrl,

        merchant.InstagramHandle,

        merchant.Status.ToString(),

        merchant.ShadowMode,

        merchant.CreatedAt);

    public static ConsentDto ToDto(this MerchantConsent consent) => new(consent.Id, consent.TermsVersion, consent.ShareOutcomesToNetwork, consent.AcceptedAt);
}
