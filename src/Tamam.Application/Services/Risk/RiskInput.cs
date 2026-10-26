namespace Tamam.Application.Services.Risk;

public record RiskInput(
    int MerchantId,

    long CurrentOrderId,

    long? CustomerId,

    long? MerchantCustomerId,

    string RecipientPhoneE164,

    string CountryCode,

    string AddressLine,

    string? City,

    int? RegionId,

    decimal TotalAmount,

    DateTime PlacedAt,

    bool ShadowMode);
