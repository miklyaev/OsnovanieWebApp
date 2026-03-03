using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrpcService1.Migrations
{
    /// <inheritdoc />
    public partial class AddOffice3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_user_t_office_TOfficeOfficeId",
                table: "t_user");

            migrationBuilder.RenameColumn(
                name: "TOfficeOfficeId",
                table: "t_user",
                newName: "office_id");

            migrationBuilder.RenameIndex(
                name: "IX_t_user_TOfficeOfficeId",
                table: "t_user",
                newName: "IX_t_user_office_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_t_office_office_id",
                table: "t_user",
                column: "office_id",
                principalTable: "t_office",
                principalColumn: "office_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_user_t_office_office_id",
                table: "t_user");

            migrationBuilder.RenameColumn(
                name: "office_id",
                table: "t_user",
                newName: "TOfficeOfficeId");

            migrationBuilder.RenameIndex(
                name: "IX_t_user_office_id",
                table: "t_user",
                newName: "IX_t_user_TOfficeOfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_t_office_TOfficeOfficeId",
                table: "t_user",
                column: "TOfficeOfficeId",
                principalTable: "t_office",
                principalColumn: "office_id");
        }
    }
}
