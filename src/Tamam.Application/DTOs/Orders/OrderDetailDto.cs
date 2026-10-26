using Tamam.Application.DTOs.Messaging;

using Tamam.Application.DTOs.Rejections;

using Tamam.Application.DTOs.Risk;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public record OrderDetailDto(
    long Id,

    string OrderNo,

    OrderStatusType StatusId,

    string StatusName,

    OrderSource Source,

    bool IsHistorical,

    string RecipientName,

    string RecipientPhoneE164,

    string? AltPhoneE164,

    string CountryCode,

    int? RegionId,

    string? City,

    string AddressLine,

    string? Landmark,

    decimal? Latitude,

    decimal? Longitude,

    string CurrencyCode,

    decimal ItemsTotal,

    decimal ShippingFee,

    decimal DiscountAmount,

    decimal TotalAmount,

    PaymentMethodType PaymentMethod,

    decimal DepositAmount,

    decimal AmountToCollect,

    DateOnly? ExpectedDeliveryDate,

    int? CarrierId,

    string? CarrierName,

    string? TrackingNo,

    DateTime? ShippedAt,

    DateTime? DeliveredAt,

    string? Notes,

    DateTime PlacedAt,

    IReadOnlyList<OrderItemDto> Items,

    IReadOnlyList<OrderStatusHistoryDto> StatusHistory,

    IReadOnlyList<DeliveryAttemptDto> DeliveryAttempts,

    RiskAssessmentDto? Risk,

    ConfirmationDto? Confirmation,

    string? RejectionSurveyLink,

    OrderOutcomeDto? Outcome);
