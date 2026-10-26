namespace Tamam.Application.DTOs.Merchants;

public record MerchantProfileDto(
    int Id,

    string BusinessName,

    string OwnerName,

    string Phone,

    string? Email,

    string CountryCode,

    string CurrencyCode,

    string DefaultLanguage,

    string? StoreUrl,

    string? InstagramHandle,

    string Status,

    bool ShadowMode,

    DateTime CreatedAt);
