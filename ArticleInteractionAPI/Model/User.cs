namespace ArticleInteractionAPI.Model;

public class User
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public virtual ICollection<ArticleLike> ArticleLikes { get; set; }
}