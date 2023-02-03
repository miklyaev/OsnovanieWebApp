using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GrpcService1.Migrations
{
    /// <inheritdoc />
    public partial class AddRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "region_id",
                table: "t_office",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "t_region",
                columns: table => new
                {
                    regionid = table.Column<int>(name: "region_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    regionname = table.Column<string>(name: "region_name", type: "text", nullable: true),
                    regioncode = table.Column<int>(name: "region_code", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_region", x => x.regionid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_office_region_id",
                table: "t_office",
                column: "region_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_office_t_region_region_id",
                table: "t_office",
                column: "region_id",
                principalTable: "t_region",
                principalColumn: "region_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_office_t_region_region_id",
                table: "t_office");

            migrationBuilder.DropTable(
                name: "t_region");

            migrationBuilder.DropIndex(
                name: "IX_t_office_region_id",
                table: "t_office");

            migrationBuilder.DropColumn(
                name: "region_id",
                table: "t_office");
        }
    }
}
