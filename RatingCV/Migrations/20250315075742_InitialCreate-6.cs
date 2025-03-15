using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatingCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "thong_tin_chi_tiet_ungvien",
                columns: table => new
                {
                    ungvienid = table.Column<int>(type: "integer", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false),
                    hoc_van = table.Column<string>(type: "text", nullable: false),
                    chung_chi = table.Column<string>(type: "text", nullable: false),
                    kinh_nghiem = table.Column<string>(type: "text", nullable: false),
                    cong_nghe = table.Column<string>(type: "text", nullable: false),
                    framework = table.Column<string>(type: "text", nullable: false),
                    data_base = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thong_tin_chi_tiet_ungvien", x => x.ungvienid);
                    table.ForeignKey(
                        name: "FK_thong_tin_chi_tiet_ungvien_cv_ungvien_ungvienid",
                        column: x => x.ungvienid,
                        principalTable: "cv_ungvien",
                        principalColumn: "ungvienid",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "thong_tin_chi_tiet_ungvien");
        }
    }
}
