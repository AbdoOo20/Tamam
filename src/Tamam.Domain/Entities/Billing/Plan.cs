namespace Tamam.Domain.Entities;

public class Plan
{
    public short Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string NameAr { get; set; } = string.Empty;

    public decimal MonthlyFee { get; set; }

    public decimal PricePerCheckedOrder { get; set; }

    public int IncludedOrders { get; set; }

    public string CurrencyCode { get; set; } = "USD";

    public bool IsActive { get; set; } = true;

    public ICollection<MerchantSubscription> Subscriptions { get; set; } = [];
}
