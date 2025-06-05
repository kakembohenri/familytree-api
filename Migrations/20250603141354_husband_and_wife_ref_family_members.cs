using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace familytree_api.Migrations
{
    /// <inheritdoc />
    public partial class husband_and_wife_ref_family_members : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_partner_users_husband_id",
                table: "partner");

            migrationBuilder.DropForeignKey(
                name: "FK_partner_users_wife_id",
                table: "partner");

            migrationBuilder.AddForeignKey(
                name: "FK_partner_family_member_husband_id",
                table: "partner",
                column: "husband_id",
                principalTable: "family_member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_partner_family_member_wife_id",
                table: "partner",
                column: "wife_id",
                principalTable: "family_member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_partner_family_member_husband_id",
                table: "partner");

            migrationBuilder.DropForeignKey(
                name: "FK_partner_family_member_wife_id",
                table: "partner");

            migrationBuilder.AddForeignKey(
                name: "FK_partner_users_husband_id",
                table: "partner",
                column: "husband_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_partner_users_wife_id",
                table: "partner",
                column: "wife_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
