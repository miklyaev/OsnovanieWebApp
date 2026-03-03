using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrpcService1.Migrations
{
    /// <inheritdoc />
    public partial class Init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_role_group_t_group_group_id",
                table: "t_role_group");

            migrationBuilder.DropForeignKey(
                name: "FK_t_role_group_t_role_role_id",
                table: "t_role_group");

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "t_role_group",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "group_id",
                table: "t_role_group",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_t_role_group_t_group_group_id",
                table: "t_role_group",
                column: "group_id",
                principalTable: "t_group",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_role_group_t_role_role_id",
                table: "t_role_group",
                column: "role_id",
                principalTable: "t_role",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_role_group_t_group_group_id",
                table: "t_role_group");

            migrationBuilder.DropForeignKey(
                name: "FK_t_role_group_t_role_role_id",
                table: "t_role_group");

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "t_role_group",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "group_id",
                table: "t_role_group",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_t_role_group_t_group_group_id",
                table: "t_role_group",
                column: "group_id",
                principalTable: "t_group",
                principalColumn: "group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_role_group_t_role_role_id",
                table: "t_role_group",
                column: "role_id",
                principalTable: "t_role",
                principalColumn: "role_id");
        }
    }
}
