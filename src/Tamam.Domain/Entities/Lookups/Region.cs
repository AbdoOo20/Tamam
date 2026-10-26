namespace Tamam.Domain.Entities;

public class Region
{
    public int Id { get; set; }

    public string CountryCode { get; set; } = string.Empty;

    public int? ParentRegionId { get; set; }

    public string NameAr { get; set; } = string.Empty;

    public string? NameEn { get; set; }

    public bool IsActive { get; set; } = true;

    public Country Country { get; set; } = null!;

    public Region? ParentRegion { get; set; }

    public ICollection<Region> Children { get; set; } = [];
}
