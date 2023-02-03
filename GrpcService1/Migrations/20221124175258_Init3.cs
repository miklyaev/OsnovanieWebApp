using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrpcService1.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_user_role_t_role_role_id",
                table: "t_user_role");

            migrationBuilder.DropForeignKey(
                name: "FK_t_user_role_t_user_user_id",
                table: "t_user_role");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "t_user_role",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "t_user_role",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_role_t_role_role_id",
                table: "t_user_role",
                column: "role_id",
                principalTable: "t_role",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_role_t_user_user_id",
                table: "t_user_role",
                column: "user_id",
                principalTable: "t_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_user_role_t_role_role_id",
                table: "t_user_role");

            migrationBuilder.DropForeignKey(
                name: "FK_t_user_role_t_user_user_id",
                table: "t_user_role");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "t_user_role",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "t_user_role",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_role_t_role_role_id",
                table: "t_user_role",
                column: "role_id",
                principalTable: "t_role",
                principalColumn: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_role_t_user_user_id",
                table: "t_user_role",
                column: "user_id",
                principalTable: "t_user",
                principalColumn: "user_id");
        }
    }
}
