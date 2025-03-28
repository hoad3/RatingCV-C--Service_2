using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatingCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "teamsizze",
                table: "du_an",
                newName: "team_size");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "team_size",
                table: "du_an",
                newName: "teamsizze");
        }
    }
}
