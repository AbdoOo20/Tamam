using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Products;

public record ProductRequest
{
    [MaxLength(60)]
    public string? Sku { get; init; }

    [Required, MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Range(0, 100000000)]
    public decimal? DefaultPrice { get; init; }

    [MaxLength(500)]
    public string? AdUrl { get; init; }

    public bool IsActive { get; init; } = true;
}
