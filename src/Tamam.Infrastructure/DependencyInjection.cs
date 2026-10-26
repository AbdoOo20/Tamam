using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

using Tamam.Application.Abstractions;

using Tamam.Infrastructure.Persistence;

using Tamam.Infrastructure.Persistence.Repositories;

namespace Tamam.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TamamDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
