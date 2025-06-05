using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace familytree_api.Migrations
{
    /// <inheritdoc />
    public partial class family_members_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "family_member",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    family_id = table.Column<int>(type: "int", nullable: false),
                    born = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    died = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    show_in_tree = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_family_member", x => x.id);
                    table.ForeignKey(
                        name: "FK_family_member_family_family_id",
                        column: x => x.family_id,
                        principalTable: "family",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_family_member_family_member_parent_id",
                        column: x => x.parent_id,
                        principalTable: "family_member",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_family_member_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_family_member_family_id",
                table: "family_member",
                column: "family_id");

            migrationBuilder.CreateIndex(
                name: "IX_family_member_parent_id",
                table: "family_member",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_family_member_user_id",
                table: "family_member",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "family_member");
        }
    }
}
