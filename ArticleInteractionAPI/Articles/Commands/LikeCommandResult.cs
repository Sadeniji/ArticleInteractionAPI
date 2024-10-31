using static ArticleInteractionAPI.Articles.Commands.LikeArticleCommandHandler;

namespace ArticleInteractionAPI.Articles.Commands;

public record LikeCommandResult
{
    public LikeStatus Status { get; init; }
    public long UpdatedCount { get; init; }
}

public enum LikeStatus
{
    Success,
    AlreadyLiked,
    RateLimited
}