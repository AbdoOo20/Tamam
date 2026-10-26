namespace Tamam.Application.DTOs.Orders;

public record OrderItemDto(long Id, long? ProductId, string ProductName, string? Variant, int Quantity, decimal UnitPrice, decimal LineTotal);
