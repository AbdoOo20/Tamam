using Microsoft.EntityFrameworkCore;

namespace Tamam.Application.Common;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var safePage = Math.Max(page, 1);

        var safeSize = Math.Clamp(pageSize, 1, 100);

        var total = await query.CountAsync(cancellationToken);

        var items = await query.Skip((safePage - 1) * safeSize).Take(safeSize).ToListAsync(cancellationToken);

        return new PagedResult<T>(items, safePage, safeSize, total);
    }

    /// <summary>
    /// Counts and pages the source query first and applies <paramref name="project" /> to the page only,
    /// so per-row subqueries in the projection (counts, max dates) run for one page instead of every row.
    /// </summary>
    public static async Task<PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(this IQueryable<TSource> query, Func<IQueryable<TSource>, IQueryable<TResult>> project, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var safePage = Math.Max(page, 1);

        var safeSize = Math.Clamp(pageSize, 1, 100);

        var total = await query.CountAsync(cancellationToken);

        var items = await project(query.Skip((safePage - 1) * safeSize).Take(safeSize)).ToListAsync(cancellationToken);

        return new PagedResult<TResult>(items, safePage, safeSize, total);
    }
}
