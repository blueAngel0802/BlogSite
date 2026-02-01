using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLikeUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Likes_BlogPostId",
                table: "Likes");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_BlogPostId_UserId",
                table: "Likes",
                columns: new[] { "BlogPostId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Likes_BlogPostId_UserId",
                table: "Likes");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_BlogPostId",
                table: "Likes",
                column: "BlogPostId");
        }
    }
}
