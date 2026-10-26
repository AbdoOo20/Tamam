using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Orders;

public record OrderItemRequest
{
    public long? ProductId { get; init; }

    [Required, MaxLength(200)]
    public string ProductName { get; init; } = string.Empty;

    [MaxLength(100)]
    public string? Variant { get; init; }

    [Range(1, 10000)]
    public int Quantity { get; init; } = 1;

    [Range(0, 100000000)]
    public decimal UnitPrice { get; init; }
}
