using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Customers;

using Tamam.Application.Services.Customers;

namespace Tamam.Api.Controllers;

[Authorize]
public class CustomersController(ICustomerService customerService) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<PagedResult<CustomerDto>>>> GetAll([FromQuery] PagedQuery query, CancellationToken cancellationToken)
    {
        return Respond(await customerService.GetAllAsync(query, cancellationToken));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Result<CustomerDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        return Respond(await customerService.GetByIdAsync(id, cancellationToken));
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<Result<CustomerDto>>> Update(long id, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        return Respond(await customerService.UpdateAsync(id, request, cancellationToken));
    }
}
