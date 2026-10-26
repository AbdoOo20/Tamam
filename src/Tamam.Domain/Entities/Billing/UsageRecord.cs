using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class UsageRecord : BaseEntity<long>
{
    public int MerchantId { get; set; }

    public long? OrderId { get; set; }

    public UsageType Type { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal UnitCost { get; set; }

    public decimal TotalCost { get; private set; }

    public DateTime OccurredAt { get; set; }

    public Merchant Merchant { get; set; } = null!;

    public Order? Order { get; set; }
}
