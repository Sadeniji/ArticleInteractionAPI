using ArticleInteractionAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ArticleInteractionAPI;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleLike> ArticleLikes { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArticleLike>()
            .HasOne(al => al.User)
            .WithMany(u => u.ArticleLikes)
            .HasForeignKey(al => al.UserId);

        modelBuilder.Entity<ArticleLike>()
            .HasOne(al => al.Article)
            .WithMany()
            .HasForeignKey(al => al.ArticleId);
    }
}