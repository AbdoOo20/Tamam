using System.ComponentModel.DataAnnotations;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public record ChangeOrderStatusRequest
{
    [EnumDataType(typeof(OrderStatusType))]
    public OrderStatusType StatusId { get; init; }

    [MaxLength(500)]
    public string? Note { get; init; }
}
