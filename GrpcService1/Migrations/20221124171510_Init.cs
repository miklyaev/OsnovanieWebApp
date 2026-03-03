using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GrpcService1.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_group",
                columns: table => new
                {
                    groupid = table.Column<int>(name: "group_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    groupname = table.Column<string>(name: "group_name", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_group", x => x.groupid);
                });

            migrationBuilder.CreateTable(
                name: "t_role",
                columns: table => new
                {
                    roleid = table.Column<int>(name: "role_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rolename = table.Column<int>(name: "role_name", type: "integer", nullable: false),
                    creationtime = table.Column<DateTime>(name: "creation_time", type: "timestamp with time zone", nullable: false),
                    offtime = table.Column<DateTime>(name: "off_time", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_role", x => x.roleid);
                });

            migrationBuilder.CreateTable(
                name: "t_user",
                columns: table => new
                {
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(name: "user_name", type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    age = table.Column<int>(type: "integer", nullable: true),
                    firstname = table.Column<string>(name: "first_name", type: "text", nullable: true),
                    lastname = table.Column<string>(name: "last_name", type: "text", nullable: true),
                    patronymic = table.Column<string>(type: "text", nullable: true),
                    creationtime = table.Column<DateTime>(name: "creation_time", type: "timestamp with time zone", nullable: false),
                    offtime = table.Column<DateTime>(name: "off_time", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "t_role_group",
                columns: table => new
                {
                    rolegroupid = table.Column<int>(name: "role_group_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleid = table.Column<int>(name: "role_id", type: "integer", nullable: true),
                    groupid = table.Column<int>(name: "group_id", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_role_group", x => x.rolegroupid);
                    table.ForeignKey(
                        name: "FK_t_role_group_t_group_group_id",
                        column: x => x.groupid,
                        principalTable: "t_group",
                        principalColumn: "group_id");
                    table.ForeignKey(
                        name: "FK_t_role_group_t_role_role_id",
                        column: x => x.roleid,
                        principalTable: "t_role",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "t_user_role",
                columns: table => new
                {
                    userroleid = table.Column<int>(name: "user_role_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: true),
                    roleid = table.Column<int>(name: "role_id", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user_role", x => x.userroleid);
                    table.ForeignKey(
                        name: "FK_t_user_role_t_role_role_id",
                        column: x => x.roleid,
                        principalTable: "t_role",
                        principalColumn: "role_id");
                    table.ForeignKey(
                        name: "FK_t_user_role_t_user_user_id",
                        column: x => x.userid,
                        principalTable: "t_user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_role_group_group_id",
                table: "t_role_group",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_role_group_role_id",
                table: "t_role_group",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_user_role_role_id",
                table: "t_user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_user_role_user_id",
                table: "t_user_role",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_role_group");

            migrationBuilder.DropTable(
                name: "t_user_role");

            migrationBuilder.DropTable(
                name: "t_group");

            migrationBuilder.DropTable(
                name: "t_role");

            migrationBuilder.DropTable(
                name: "t_user");
        }
    }
}
