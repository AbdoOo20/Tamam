using System.ComponentModel.DataAnnotations;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public record UpdateOrderRequest
{
    [Required, MaxLength(150)]
    public string RecipientName { get; init; } = string.Empty;

    [MaxLength(25)]
    public string? AltPhone { get; init; }

    public int? RegionId { get; init; }

    [MaxLength(100)]
    public string? City { get; init; }

    [Required, MaxLength(500)]
    public string AddressLine { get; init; } = string.Empty;

    [MaxLength(200)]
    public string? Landmark { get; init; }

    [Range(-90, 90)]
    public decimal? Latitude { get; init; }

    [Range(-180, 180)]
    public decimal? Longitude { get; init; }

    [Range(0, 100000000)]
    public decimal ShippingFee { get; init; }

    [Range(0, 100000000)]
    public decimal DiscountAmount { get; init; }

    [EnumDataType(typeof(PaymentMethodType))]
    public PaymentMethodType PaymentMethod { get; init; } = PaymentMethodType.Cod;

    [Range(0, 100000000)]
    public decimal DepositAmount { get; init; }

    public DateOnly? ExpectedDeliveryDate { get; init; }

    public int? CarrierId { get; init; }

    [MaxLength(100)]
    public string? TrackingNo { get; init; }

    [MaxLength(1000)]
    public string? Notes { get; init; }
}
