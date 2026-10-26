using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public record OrderListItemDto(
    long Id,

    string OrderNo,

    string RecipientName,

    string RecipientPhoneE164,

    string? City,

    string CurrencyCode,

    decimal TotalAmount,

    decimal AmountToCollect,

    OrderStatusType StatusId,

    string StatusName,

    byte? LatestRiskScore,

    RiskLevel? LatestRiskLevel,

    DateOnly? ExpectedDeliveryDate,

    DateTime PlacedAt);
