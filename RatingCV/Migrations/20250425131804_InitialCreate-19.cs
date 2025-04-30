using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RatingCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "danh_gia_theo_tieu_chi",
                columns: table => new
                {
                    id_danh_gia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ungvien_id = table.Column<int>(type: "integer", nullable: false),
                    id_rating = table.Column<int>(type: "integer", nullable: false),
                    id_session = table.Column<int>(type: "integer", nullable: false),
                    ten_file = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_danh_gia_theo_tieu_chi", x => x.id_danh_gia);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "danh_gia_theo_tieu_chi");
        }
    }
}
