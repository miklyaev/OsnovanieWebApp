using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrpcService1.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Age_Index",
                table: "t_user",
                column: "age");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Age_Index",
                table: "t_user");
        }
    }
}
