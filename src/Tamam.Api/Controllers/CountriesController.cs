using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Abstractions;

using Tamam.Application.DTOs.Countries;

using Tamam.Domain.Entities;

namespace Tamam.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CountryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var countries = await unitOfWork.Repository<Country>().ListAsync(cancellationToken: cancellationToken);

        var result = countries.Select(country => new CountryDto(country.Id, country.NameAr, country.NameEn, country.PhoneCode, country.CurrencyCode)).ToList();

        return Ok(result);
    }
}
