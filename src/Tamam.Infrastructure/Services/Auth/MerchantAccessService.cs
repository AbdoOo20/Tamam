using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Caching.Memory;

using Microsoft.Extensions.DependencyInjection;

using Tamam.Application.Abstractions;

using Tamam.Domain.Enums;

using Tamam.Infrastructure.Persistence;

namespace Tamam.Infrastructure.Services.Auth;

public class MerchantAccessService(IMemoryCache cache, IServiceScopeFactory scopeFactory) : IMerchantAccessService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(30);

    public async Task<bool> IsActiveAsync(int merchantId, CancellationToken cancellationToken = default)
    {
        if (cache.TryGetValue(CacheKey(merchantId), out bool cached))
        {
            return cached;
        }

        using var scope = scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TamamDbContext>();

        var status = await context.Merchants
            .AsNoTracking()
            .Where(merchant => merchant.Id == merchantId)
            .Select(merchant => (MerchantStatus?)merchant.Status)
            .FirstOrDefaultAsync(cancellationToken);

        var isActive = status is not null and not MerchantStatus.Suspended and not MerchantStatus.Closed;

        cache.Set(CacheKey(merchantId), isActive, CacheDuration);

        return isActive;
    }

    public void Forget(int merchantId)
    {
        cache.Remove(CacheKey(merchantId));
    }

    private static string CacheKey(int merchantId)
    {
        return $"merchant-active:{merchantId}";
    }
}
