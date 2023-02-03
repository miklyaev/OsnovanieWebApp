using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GrpcService1.Migrations
{
    /// <inheritdoc />
    public partial class AddOffice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfficeId",
                table: "t_user",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "t_office",
                columns: table => new
                {
                    officeid = table.Column<int>(name: "office_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    officename = table.Column<string>(name: "office_name", type: "text", nullable: true),
                    creationtime = table.Column<DateTime>(name: "creation_time", type: "timestamp with time zone", nullable: false),
                    offtime = table.Column<DateTime>(name: "off_time", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_office", x => x.officeid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_user_OfficeId",
                table: "t_user",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_t_office_OfficeId",
                table: "t_user",
                column: "OfficeId",
                principalTable: "t_office",
                principalColumn: "office_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_user_t_office_OfficeId",
                table: "t_user");

            migrationBuilder.DropTable(
                name: "t_office");

            migrationBuilder.DropIndex(
                name: "IX_t_user_OfficeId",
                table: "t_user");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "t_user");
        }
    }
}
