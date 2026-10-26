using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class OrderItem : BaseEntity<long>
{
    public long OrderId { get; set; }

    public long? ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string? Variant { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; private set; }

    public Order Order { get; set; } = null!;

    public Product? Product { get; set; }
}
