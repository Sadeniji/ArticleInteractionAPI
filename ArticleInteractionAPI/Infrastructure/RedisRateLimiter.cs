using Microsoft.Extensions.Caching.Distributed;

namespace ArticleInteractionAPI.Infrastructure;

public class RedisRateLimiter : IRateLimiter
{
    private readonly IDistributedCache _cache;
    private const int MAX_REQUESTS = 10;
    private const int WINDOW_SECONDS = 60;

    public RedisRateLimiter(IDistributedCache cache)
    {
        _cache = cache;
    }
    public async Task<bool> CheckRateLimit(string userId)
    {
        var key = $"ratelimit:{userId}";
        var count = await _cache.GetStringAsync(key);

        if (count == null)
        {
            await _cache.SetStringAsync(
                key,
                "1",
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(WINDOW_SECONDS)
                });
            return true;
        }

        var currentCount = int.Parse(count);
        if (currentCount >= MAX_REQUESTS)
            return false;

        await _cache.SetStringAsync(
            key,
            (currentCount + 1).ToString(),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(WINDOW_SECONDS)
            });

        return true;
    }
}