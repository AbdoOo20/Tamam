using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Carriers;

using Tamam.Application.Services.Carriers;

namespace Tamam.Api.Controllers;

[Authorize]
public class CarriersController(ICarrierService carrierService) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<IReadOnlyList<CarrierDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return Respond(await carrierService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Result<CarrierDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        return Respond(await carrierService.GetByIdAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<Result<CarrierDto>>> Create(CarrierRequest request, CancellationToken cancellationToken)
    {
        return Respond(await carrierService.CreateAsync(request, cancellationToken));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Result<CarrierDto>>> Update(int id, CarrierRequest request, CancellationToken cancellationToken)
    {
        return Respond(await carrierService.UpdateAsync(id, request, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Result>> Delete(int id, CancellationToken cancellationToken)
    {
        return Respond(await carrierService.DeleteAsync(id, cancellationToken));
    }
}
