using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class OrderStatus
{
    public OrderStatusType Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string NameAr { get; set; } = string.Empty;

    public string NameEn { get; set; } = string.Empty;

    public bool IsFinal { get; set; }

    public byte SortOrder { get; set; }
}
