namespace ArticleInteractionAPI.Model;

public class ArticleLike
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public Guid UserId { get; set; }
    public virtual Article Article { get; set; }
    public virtual User User { get; set; } 
}