using Tamam.Application.Common;

using Tamam.Application.DTOs.Products;

namespace Tamam.Application.Services.Products;

public interface IProductService
{
    Task<Result<PagedResult<ProductDto>>> GetAllAsync(ProductQuery query, CancellationToken cancellationToken = default);

    Task<Result<ProductDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Result<ProductDto>> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default);

    Task<Result<ProductDto>> UpdateAsync(long id, ProductRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
