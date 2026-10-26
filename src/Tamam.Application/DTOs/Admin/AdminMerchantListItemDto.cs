using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Admin;

public record AdminMerchantListItemDto(
    int Id,

    string BusinessName,

    string OwnerName,

    string Phone,

    string? Email,

    string CountryCode,

    MerchantStatus Status,

    bool ShadowMode,

    int OrdersCount,

    DateTime CreatedAt);
