using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GrpcService1.Migrations
{
    /// <inheritdoc />
    public partial class Book4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_author",
                columns: table => new
                {
                    authorid = table.Column<int>(name: "author_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    age = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_author", x => x.authorid);
                });

            migrationBuilder.CreateTable(
                name: "t_book",
                columns: table => new
                {
                    bookid = table.Column<int>(name: "book_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    pages = table.Column<int>(type: "integer", nullable: true),
                    authorid = table.Column<int>(name: "author_id", type: "integer", nullable: true),
                    issuedate = table.Column<DateTime>(name: "issue_date", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_book", x => x.bookid);
                    table.ForeignKey(
                        name: "FK_t_book_t_author_author_id",
                        column: x => x.authorid,
                        principalTable: "t_author",
                        principalColumn: "author_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_book_author_id",
                table: "t_book",
                column: "author_id");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_book");

            migrationBuilder.DropTable(
                name: "t_role_group");

            migrationBuilder.DropTable(
                name: "t_user_role");

            migrationBuilder.DropTable(
                name: "t_author");

            migrationBuilder.DropTable(
                name: "t_group");

            migrationBuilder.DropTable(
                name: "t_role");

            migrationBuilder.DropTable(
                name: "t_user");

            migrationBuilder.DropTable(
                name: "t_office");

            migrationBuilder.DropTable(
                name: "t_region");
        }
    }
}
