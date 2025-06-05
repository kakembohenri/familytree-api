using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace familytree_api.Migrations
{
    /// <inheritdoc />
    public partial class father_and_mother_to_family_members : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "father_id",
                table: "family_member",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "family_member",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "mother_id",
                table: "family_member",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_family_member_father_id",
                table: "family_member",
                column: "father_id");

            migrationBuilder.CreateIndex(
                name: "IX_family_member_mother_id",
                table: "family_member",
                column: "mother_id");

            migrationBuilder.AddForeignKey(
                name: "FK_family_member_family_member_father_id",
                table: "family_member",
                column: "father_id",
                principalTable: "family_member",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_family_member_family_member_mother_id",
                table: "family_member",
                column: "mother_id",
                principalTable: "family_member",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_family_member_family_member_father_id",
                table: "family_member");

            migrationBuilder.DropForeignKey(
                name: "FK_family_member_family_member_mother_id",
                table: "family_member");

            migrationBuilder.DropIndex(
                name: "IX_family_member_father_id",
                table: "family_member");

            migrationBuilder.DropIndex(
                name: "IX_family_member_mother_id",
                table: "family_member");

            migrationBuilder.DropColumn(
                name: "father_id",
                table: "family_member");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "family_member");

            migrationBuilder.DropColumn(
                name: "mother_id",
                table: "family_member");
        }
    }
}
