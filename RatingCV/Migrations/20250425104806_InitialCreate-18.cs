using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatingCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rating_cv_cv_ungvien_ungvien_id",
                table: "rating_cv");

            migrationBuilder.DropIndex(
                name: "IX_rating_cv_ungvien_id",
                table: "rating_cv");

            migrationBuilder.RenameColumn(
                name: "ungvien_id",
                table: "rating_cv",
                newName: "session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "session_id",
                table: "rating_cv",
                newName: "ungvien_id");

            migrationBuilder.CreateIndex(
                name: "IX_rating_cv_ungvien_id",
                table: "rating_cv",
                column: "ungvien_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_rating_cv_cv_ungvien_ungvien_id",
                table: "rating_cv",
                column: "ungvien_id",
                principalTable: "cv_ungvien",
                principalColumn: "ungvienid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
