namespace ArticleInteractionAPI.Infrastructure;

public interface IRateLimiter
{
    Task<bool> CheckRateLimit(string userId);
}