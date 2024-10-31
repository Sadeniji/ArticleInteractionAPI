using MediatR;

namespace ArticleInteractionAPI.Articles.Queries;

public record GetArticleLikeCountQuery(Guid ArticleId) : IRequest<int>;