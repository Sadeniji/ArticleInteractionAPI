using ArticleInteractionAPI.Articles.Commands;
using ArticleInteractionAPI.Articles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArticleInteractionAPI.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ISender _sender;

        public ArticlesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("{userId}/likes/{articleId}")]
        public async Task<IActionResult> LikeArticle(Guid userId, Guid articleId, CancellationToken cancellationToken)
        {
            var success = await _sender.Send(new LikeArticleCommand(userId, articleId), cancellationToken);

            if (!success)
                return BadRequest("User has already liked this article or user does not exist.");

            return Ok("Like added successfully.");
        }

        [HttpGet("{articleId}/likes")]
        public async Task<IActionResult> GetLikeCount(Guid articleId)
        {
            return Ok(await _sender.Send(new GetArticleLikeCountQuery(articleId)));
        }
    }
}
