using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Countries;

using Tamam.Application.Services.Countries;

namespace Tamam.Api.Controllers;

public class CountriesController(ICountryService countryService) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<IReadOnlyList<CountryDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return Respond(await countryService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<Result<CountryDto>>> GetByCode(string code, CancellationToken cancellationToken)
    {
        return Respond(await countryService.GetByCodeAsync(code, cancellationToken));
    }
}
