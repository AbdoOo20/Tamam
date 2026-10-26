using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Products;

using Tamam.Application.Services.Products;

namespace Tamam.Api.Controllers;

[Authorize]
public class ProductsController(IProductService productService) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<PagedResult<ProductDto>>>> GetAll([FromQuery] ProductQuery query, CancellationToken cancellationToken)
    {
        return Respond(await productService.GetAllAsync(query, cancellationToken));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Result<ProductDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        return Respond(await productService.GetByIdAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<Result<ProductDto>>> Create(ProductRequest request, CancellationToken cancellationToken)
    {
        return Respond(await productService.CreateAsync(request, cancellationToken));
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<Result<ProductDto>>> Update(long id, ProductRequest request, CancellationToken cancellationToken)
    {
        return Respond(await productService.UpdateAsync(id, request, cancellationToken));
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Result>> Delete(long id, CancellationToken cancellationToken)
    {
        return Respond(await productService.DeleteAsync(id, cancellationToken));
    }
}
