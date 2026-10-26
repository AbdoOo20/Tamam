using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class OrderStatus : BaseEntity<OrderStatusType>
{
    public string Code { get; set; } = string.Empty;

    public string NameAr { get; set; } = string.Empty;

    public string NameEn { get; set; } = string.Empty;

    public bool IsFinal { get; set; }

    public byte SortOrder { get; set; }
}
