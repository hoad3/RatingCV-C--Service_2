using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RatingCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rating_cv",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ungvien_id = table.Column<int>(type: "integer", nullable: false),
                    trang_thai = table.Column<string>(type: "text", nullable: false),
                    file_json = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rating_cv", x => x.id);
                    table.ForeignKey(
                        name: "FK_rating_cv_cv_ungvien_ungvien_id",
                        column: x => x.ungvien_id,
                        principalTable: "cv_ungvien",
                        principalColumn: "ungvienid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rating_cv_ungvien_id",
                table: "rating_cv",
                column: "ungvien_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rating_cv");
        }
    }
}
