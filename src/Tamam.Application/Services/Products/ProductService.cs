using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Products;

using Tamam.Application.Mappings;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Products;

public class ProductService(IUnitOfWork unitOfWork, IUserAppService userAppService) : IProductService
{
    public async Task<Result<PagedResult<ProductDto>>> GetAllAsync(ProductQuery query, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var products = unitOfWork.Repository<Product>().Query().Where(product => product.MerchantId == merchantId);

        if (query.IsActive is { } isActive)
        {
            products = products.Where(product => product.IsActive == isActive);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();

            products = products.Where(product => product.Name.Contains(term) || (product.Sku != null && product.Sku.Contains(term)));
        }

        var page = await products
            .OrderBy(product => product.Name)
            .Select(product => new ProductDto(product.Id, product.Sku, product.Name, product.DefaultPrice, product.AdUrl, product.IsActive))
            .ToPagedResultAsync(query.Page, query.PageSize, cancellationToken);

        return Result<PagedResult<ProductDto>>.Ok(page);
    }

    public async Task<Result<ProductDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var product = await unitOfWork.Repository<Product>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        return product is null ? Result<ProductDto>.NotFound("Product was not found.") : Result<ProductDto>.Ok(product.ToDto());
    }

    public async Task<Result<ProductDto>> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var sku = request.Sku?.Trim();

        if (!string.IsNullOrEmpty(sku) && await unitOfWork.Repository<Product>().AnyAsync(item => item.MerchantId == merchantId && item.Sku == sku, cancellationToken))
        {
            return Result<ProductDto>.Conflict("A product with the same SKU already exists.");
        }

        var product = new Product
        {
            MerchantId = merchantId,

            Sku = sku,

            Name = request.Name.Trim(),

            DefaultPrice = request.DefaultPrice,

            AdUrl = request.AdUrl?.Trim(),

            IsActive = request.IsActive
        };

        await unitOfWork.Repository<Product>().AddAsync(product, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProductDto>.Created(product.ToDto());
    }

    public async Task<Result<ProductDto>> UpdateAsync(long id, ProductRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var product = await unitOfWork.Repository<Product>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        if (product is null)
        {
            return Result<ProductDto>.NotFound("Product was not found.");
        }

        var sku = request.Sku?.Trim();

        if (!string.IsNullOrEmpty(sku) && await unitOfWork.Repository<Product>().AnyAsync(item => item.MerchantId == merchantId && item.Sku == sku && item.Id != id, cancellationToken))
        {
            return Result<ProductDto>.Conflict("A product with the same SKU already exists.");
        }

        product.Sku = sku;

        product.Name = request.Name.Trim();

        product.DefaultPrice = request.DefaultPrice;

        product.AdUrl = request.AdUrl?.Trim();

        product.IsActive = request.IsActive;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProductDto>.Ok(product.ToDto());
    }

    public async Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var product = await unitOfWork.Repository<Product>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        if (product is null)
        {
            return Result.NotFound("Product was not found.");
        }

        unitOfWork.Repository<Product>().Remove(product);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok("Product deleted.");
    }
}
