namespace Tamam.Domain.Entities;

public class Product
{
    public long Id { get; set; }

    public int MerchantId { get; set; }

    public string? Sku { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal? DefaultPrice { get; set; }

    public string? AdUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Merchant Merchant { get; set; } = null!;
}
