using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ArticleInteractionAPI.Articles.Queries;

public class GetArticleLikeCountQueryHandler : IRequestHandler<GetArticleLikeCountQuery, int>
{
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;

    public GetArticleLikeCountQueryHandler(ApplicationDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<int> Handle(GetArticleLikeCountQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"ArticleLikes_{request.ArticleId}";
        var cachedArticleLikeCount = await _cache.GetStringAsync(cacheKey, token: cancellationToken);

        if (cachedArticleLikeCount != null)
        {
            return int.Parse(cachedArticleLikeCount);
        }

        var articleLikeCount = await _context.ArticleLikes.CountAsync(al => al.ArticleId == request.ArticleId, cancellationToken: cancellationToken);

        await _cache.SetStringAsync(cacheKey, articleLikeCount.ToString(), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        }, token: cancellationToken);

        return articleLikeCount;
    }
}