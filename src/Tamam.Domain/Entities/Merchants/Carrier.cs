namespace Tamam.Domain.Entities;

public class Carrier
{
    public int Id { get; set; }

    public int? MerchantId { get; set; }

    public string? CountryCode { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? TrackingUrlTemplate { get; set; }

    public bool IsActive { get; set; } = true;

    public Merchant? Merchant { get; set; }

    public Country? Country { get; set; }

    public ICollection<Order> Orders { get; set; } = [];
}
