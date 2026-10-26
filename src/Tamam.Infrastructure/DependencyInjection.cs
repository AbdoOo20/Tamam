using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using Tamam.Application.Abstractions;

using Tamam.Application.Auth;

using Tamam.Application.Staff;

using Tamam.Domain.Entities;

using Tamam.Infrastructure.Identity;

using Tamam.Infrastructure.Persistence;

using Tamam.Infrastructure.Persistence.Repositories;

namespace Tamam.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TamamDbContext>(options => options
            .UseSqlServer(connectionString)
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning)));

        services.AddIdentityCore<User>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequiredLength = 8;

            options.Password.RequireNonAlphanumeric = false;

            options.Lockout.MaxFailedAccessAttempts = 5;
        })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<TamamDbContext>();

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IStaffService, StaffService>();

        return services;
    }
}
