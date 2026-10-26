using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Orders;

public record UpdateOrderItemsRequest
{
    [Required, MinLength(1), MaxLength(100)]
    public List<OrderItemRequest> Items { get; init; } = [];
}
