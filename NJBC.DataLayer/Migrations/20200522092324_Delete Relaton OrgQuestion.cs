using Microsoft.EntityFrameworkCore.Migrations;

namespace NJBC.DataLayer.Migrations
{
    public partial class DeleteRelatonOrgQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgQuestion_Users_UserId",
                table: "OrgQuestion");

            migrationBuilder.DropIndex(
                name: "IX_OrgQuestion_UserId",
                table: "OrgQuestion");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrgQuestion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "OrgQuestion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgQuestion_UserId",
                table: "OrgQuestion",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgQuestion_Users_UserId",
                table: "OrgQuestion",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
