using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class Product : BaseEntity<long>, ISearchable
{
    public int MerchantId { get; set; }

    public string? Sku { get; set; }

    public string Name { get; set; } = string.Empty;

    public string SearchText { get; set; } = string.Empty;

    public decimal? DefaultPrice { get; set; }

    public string? AdUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public Merchant Merchant { get; set; } = null!;

    public string BuildSearchText() => ArabicSearch.Build(Name, Sku);
}
