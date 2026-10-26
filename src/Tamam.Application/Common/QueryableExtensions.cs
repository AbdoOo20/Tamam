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
}
