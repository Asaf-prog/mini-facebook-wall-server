using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace facebook_wall_project.Migrations
{
    /// <inheritdoc />
    public partial class addUserIdToLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Likes");
        }
    }
}
