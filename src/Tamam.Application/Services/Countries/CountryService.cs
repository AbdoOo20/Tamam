using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Countries;

using Tamam.Application.Mappings;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Countries;

public class CountryService(IUnitOfWork unitOfWork) : ICountryService
{
    public async Task<Result<IReadOnlyList<CountryDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var countries = await unitOfWork.Repository<Country>().ListAsync(country => country.IsActive, cancellationToken);

        IReadOnlyList<CountryDto> dtos = countries.Select(country => country.ToDto()).ToList();

        return Result<IReadOnlyList<CountryDto>>.Ok(dtos);
    }

    public async Task<Result<CountryDto>> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var country = await unitOfWork.Repository<Country>().GetByIdAsync(code.Trim().ToUpperInvariant(), cancellationToken);

        return country is null

        ? Result<CountryDto>.NotFound($"Country '{code}' was not found.")

        : Result<CountryDto>.Ok(country.ToDto());
    }
}
