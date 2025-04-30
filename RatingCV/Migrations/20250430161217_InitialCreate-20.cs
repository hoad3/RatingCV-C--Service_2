using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatingCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ungvien_id",
                table: "danh_gia_theo_tieu_chi",
                newName: "id_ungvien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id_ungvien",
                table: "danh_gia_theo_tieu_chi",
                newName: "ungvien_id");
        }
    }
}
