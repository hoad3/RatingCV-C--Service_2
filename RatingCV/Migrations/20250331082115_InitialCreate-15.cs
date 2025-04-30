using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatingCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "github",
                table: "du_an");

            migrationBuilder.RenameColumn(
                name: "github_link",
                table: "github",
                newName: "github");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "github",
                table: "github",
                newName: "github_link");

            migrationBuilder.AddColumn<string>(
                name: "github",
                table: "du_an",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
