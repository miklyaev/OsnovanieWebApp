using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrpcService1.Migrations
{
    /// <inheritdoc />
    public partial class AddOffice2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_user_t_office_OfficeId",
                table: "t_user");

            migrationBuilder.RenameColumn(
                name: "OfficeId",
                table: "t_user",
                newName: "TOfficeOfficeId");

            migrationBuilder.RenameIndex(
                name: "IX_t_user_OfficeId",
                table: "t_user",
                newName: "IX_t_user_TOfficeOfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_t_office_TOfficeOfficeId",
                table: "t_user",
                column: "TOfficeOfficeId",
                principalTable: "t_office",
                principalColumn: "office_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_user_t_office_TOfficeOfficeId",
                table: "t_user");

            migrationBuilder.RenameColumn(
                name: "TOfficeOfficeId",
                table: "t_user",
                newName: "OfficeId");

            migrationBuilder.RenameIndex(
                name: "IX_t_user_TOfficeOfficeId",
                table: "t_user",
                newName: "IX_t_user_OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_t_office_OfficeId",
                table: "t_user",
                column: "OfficeId",
                principalTable: "t_office",
                principalColumn: "office_id");
        }
    }
}
