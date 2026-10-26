using Microsoft.Extensions.DependencyInjection;

using Tamam.Application.Services.Countries;

namespace Tamam.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICountryService, CountryService>();

        return services;
    }
}
