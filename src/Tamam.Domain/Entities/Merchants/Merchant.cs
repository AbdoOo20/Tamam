using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class Merchant : BaseEntity<int>
{
    public string BusinessName { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string CountryCode { get; set; } = string.Empty;

    public string CurrencyCode { get; set; } = string.Empty;

    public string DefaultLanguage { get; set; } = "ar-EG";

    public string? StoreUrl { get; set; }

    public string? InstagramHandle { get; set; }

    public MerchantStatus Status { get; set; } = MerchantStatus.Trial;

    public bool ShadowMode { get; set; } = true;

    public byte[]? ApiKeyHash { get; set; }

    public Country Country { get; set; } = null!;

    public ICollection<User> Users { get; set; } = [];

    public ICollection<MerchantConsent> Consents { get; set; } = [];

    public ICollection<Carrier> Carriers { get; set; } = [];

    public ICollection<Product> Products { get; set; } = [];

    public ICollection<MerchantCustomer> MerchantCustomers { get; set; } = [];

    public ICollection<OrderImport> Imports { get; set; } = [];

    public ICollection<Order> Orders { get; set; } = [];

    public ICollection<MerchantQualityStat> QualityStats { get; set; } = [];

    public ICollection<MerchantSubscription> Subscriptions { get; set; } = [];
}
