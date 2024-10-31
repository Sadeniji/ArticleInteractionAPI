using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleInteractionAPI.Migrations
{
    public partial class RemoveLikeCountFromArticleLikeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "ArticleLikes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "ArticleLikes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
