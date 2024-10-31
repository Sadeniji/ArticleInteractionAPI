using ArticleInteractionAPI.Infrastructure;
using ArticleInteractionAPI.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ArticleInteractionAPI.Articles.Commands;

public class LikeArticleCommandHandler : IRequestHandler<LikeArticleCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly IRateLimiter _rateLimiter;
    private readonly IDistributedCache _cache;

    public LikeArticleCommandHandler(ApplicationDbContext context, IDistributedCache cache, IRateLimiter rateLimiter)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _rateLimiter = rateLimiter ?? throw new ArgumentNullException(nameof(rateLimiter));
    }

    public async Task<bool> Handle(LikeArticleCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

        if (user == null)
        {
            return false;
        }

        if (!await _rateLimiter.CheckRateLimit(request.UserId.ToString()))
        {
            return false;
        }
        var hasUserLikedArticle = await _context.ArticleLikes.AnyAsync(al => al.ArticleId == request.ArticleId && al.UserId == request.UserId, cancellationToken);

        if (hasUserLikedArticle)
        {
            return false;
        }

        var addUserLike = new ArticleLike
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ArticleId = request.ArticleId
        };
        await _context.ArticleLikes.AddAsync(addUserLike, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Update cached like count
        var cacheKey = $"ArticleLikes_{request.ArticleId}";
        var updatedLikeCount = await _context.ArticleLikes.CountAsync((al => al.ArticleId == request.ArticleId), cancellationToken: cancellationToken);
        await _cache.SetStringAsync(cacheKey, updatedLikeCount.ToString(), token: cancellationToken);

        return true;
    }
}