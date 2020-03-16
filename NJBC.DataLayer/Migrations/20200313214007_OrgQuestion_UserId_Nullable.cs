using Microsoft.EntityFrameworkCore.Migrations;

namespace NJBC.DataLayer.Migrations
{
    public partial class OrgQuestion_UserId_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgQuestion_Users_UserId",
                table: "OrgQuestion");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "OrgQuestion",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_OrgQuestion_Users_UserId",
                table: "OrgQuestion",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgQuestion_Users_UserId",
                table: "OrgQuestion");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "OrgQuestion",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgQuestion_Users_UserId",
                table: "OrgQuestion",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
