using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;

using Tamam.Domain.Common;

using Tamam.Domain.Entities;

using Tamam.Infrastructure.Persistence;

namespace Tamam.Infrastructure.Seeding;

/// <summary>Fills <see cref="ISearchable.SearchText" /> for rows that existed before the column was added. Does nothing once every row has it.</summary>
public static class SearchTextBackfill
{
    private const int BatchSize = 500;

    public static async Task BackfillSearchTextAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TamamDbContext>();

        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SearchTextBackfill");

        try
        {
            var orders = await BackfillAsync(context, context.Orders, cancellationToken);

            var customers = await BackfillAsync(context, context.MerchantCustomers, cancellationToken);

            var products = await BackfillAsync(context, context.Products, cancellationToken);

            if (orders + customers + products > 0)
            {
                logger.LogInformation("Search text rebuilt for {Orders} orders, {Customers} customers and {Products} products.", orders, customers, products);
            }
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not rebuild the search text. Run the pending migrations and restart.");
        }
    }

    private static async Task<int> BackfillAsync<T>(TamamDbContext context, DbSet<T> set, CancellationToken cancellationToken) where T : class, ISearchable
    {
        var total = 0;

        while (true)
        {
            var rows = await set.Where(row => row.SearchText == string.Empty).Take(BatchSize).ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                return total;
            }

            foreach (var row in rows)
            {
                row.SearchText = row.BuildSearchText();
            }

            await context.SaveChangesAsync(cancellationToken);

            context.ChangeTracker.Clear();

            total += rows.Count;
        }
    }
}
