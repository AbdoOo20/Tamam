namespace Tamam.Application.DTOs.Products;

public record ProductDto(long Id, string? Sku, string Name, decimal? DefaultPrice, string? AdUrl, bool IsActive);
