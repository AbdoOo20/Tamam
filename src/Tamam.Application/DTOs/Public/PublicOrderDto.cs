using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Public;

public record PublicOrderDto(
    PublicLinkPurpose Purpose,

    string StoreName,

    string RecipientName,

    string CurrencyCode,

    decimal TotalAmount,

    decimal AmountToCollect,

    string? City,

    string AddressLine,

    DateOnly? ExpectedDeliveryDate,

    OrderStatusType Status,

    bool CanRespond,

    DateTime ExpiresAt,

    IReadOnlyList<PublicOrderItemDto> Items);
