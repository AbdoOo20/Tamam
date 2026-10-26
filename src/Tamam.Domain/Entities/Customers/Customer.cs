using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class Customer : BaseEntity<long>
{
    public byte[] PhoneHash { get; set; } = [];

    public string CountryCode { get; set; } = string.Empty;

    public bool IsOptedOut { get; set; }

    public DateTime? OptedOutAt { get; set; }

    public DateTime FirstSeenAt { get; set; }

    public Country Country { get; set; } = null!;

    public ICollection<MerchantCustomer> MerchantCustomers { get; set; } = [];

    public CustomerRiskProfile? RiskProfile { get; set; }

    public ICollection<Order> Orders { get; set; } = [];
}
