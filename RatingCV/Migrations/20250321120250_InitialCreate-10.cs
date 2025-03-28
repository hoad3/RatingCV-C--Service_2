using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RatingCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "du_an",
                columns: table => new
                {
                    du_an_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    ten_du_an = table.Column<string>(type: "text", nullable: false),
                    mo_ta = table.Column<string>(type: "text", nullable: false),
                    ngay_bat_dau = table.Column<string>(type: "text", nullable: false),
                    ngay_ket_thuc = table.Column<string>(type: "text", nullable: false),
                    teamsizze = table.Column<int>(type: "integer", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    github = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_du_an", x => x.du_an_id);
                    table.ForeignKey(
                        name: "FK_du_an_cv_ungvien_userid",
                        column: x => x.userid,
                        principalTable: "cv_ungvien",
                        principalColumn: "ungvienid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_du_an_userid",
                table: "du_an",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "du_an");
        }
    }
}
