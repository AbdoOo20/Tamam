using Tamam.Application.DTOs.Products;

using Tamam.Domain.Entities;

namespace Tamam.Application.Mappings;

public static class ProductMappings
{
    public static ProductDto ToDto(this Product product) => new(product.Id, product.Sku, product.Name, product.DefaultPrice, product.AdUrl, product.IsActive);
}
