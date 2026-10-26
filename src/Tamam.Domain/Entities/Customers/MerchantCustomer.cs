using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class MerchantCustomer : BaseEntity<long>
{
    public int MerchantId { get; set; }

    public long CustomerId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneE164 { get; set; } = string.Empty;

    public string? AltPhoneE164 { get; set; }

    public bool? HasWhatsApp { get; set; }

    public string? Notes { get; set; }

    public Merchant Merchant { get; set; } = null!;

    public Customer Customer { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = [];
}
