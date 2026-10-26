using Tamam.Application.Common;

using Tamam.Application.DTOs.Countries;

namespace Tamam.Application.Services.Countries;

public interface ICountryService
{
    Task<Result<IReadOnlyList<CountryDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<CountryDto>> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
