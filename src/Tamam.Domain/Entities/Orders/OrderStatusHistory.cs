using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class OrderStatusHistory
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public OrderStatusType? FromStatusId { get; set; }

    public OrderStatusType ToStatusId { get; set; }

    public ActorType ChangedByType { get; set; }

    public int? ChangedByUserId { get; set; }

    public string? Note { get; set; }

    public DateTime ChangedAt { get; set; }

    public Order Order { get; set; } = null!;

    public OrderStatus? FromStatus { get; set; }

    public OrderStatus ToStatus { get; set; } = null!;

    public User? ChangedByUser { get; set; }
}
