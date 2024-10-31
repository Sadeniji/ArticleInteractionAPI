using MediatR;

namespace ArticleInteractionAPI.Articles.Commands;

public record LikeArticleCommand(Guid UserId, Guid ArticleId) : IRequest<bool>;