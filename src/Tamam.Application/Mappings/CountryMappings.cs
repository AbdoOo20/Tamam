using Tamam.Application.DTOs.Countries;

using Tamam.Domain.Entities;

namespace Tamam.Application.Mappings;

public static class CountryMappings
{
    public static CountryDto ToDto(this Country country) => new(country.Id, country.NameAr, country.NameEn, country.PhoneCode, country.CurrencyCode);
}
