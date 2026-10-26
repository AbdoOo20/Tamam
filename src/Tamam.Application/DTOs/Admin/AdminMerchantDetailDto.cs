using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Admin;

public record AdminMerchantDetailDto(
    int Id,

    string BusinessName,

    string OwnerName,

    string Phone,

    string? Email,

    string CountryCode,

    string CurrencyCode,

    string? StoreUrl,

    MerchantStatus Status,

    bool ShadowMode,

    bool HasApiKey,

    string? PlanCode,

    SubscriptionStatus? SubscriptionStatus,

    int UsersCount,

    int OrdersCount,

    int OrdersLast30Days,

    int RefusedLast30Days,

    DateTime CreatedAt);
