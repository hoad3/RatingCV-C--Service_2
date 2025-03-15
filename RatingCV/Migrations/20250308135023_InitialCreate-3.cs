using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatingCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ten_ung_vien",
                table: "cv_ungvien",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "sdt",
                table: "cv_ungvien",
                newName: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phone",
                table: "cv_ungvien",
                newName: "ten_ung_vien");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "cv_ungvien",
                newName: "sdt");
        }
    }
}
