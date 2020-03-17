using Microsoft.EntityFrameworkCore.Migrations;

namespace NJBC.DataLayer.Migrations
{
    public partial class addVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__RelCommen__RELQ___3C69FB99",
                table: "RelComment");

            migrationBuilder.AlterColumn<int>(
                name: "RELQ_ID",
                table: "RelComment",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__RelCommen__RELQ___3C69FB99",
                table: "RelComment",
                column: "RELQ_ID",
                principalTable: "RelQuestion",
                principalColumn: "RELQ_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__RelCommen__RELQ___3C69FB99",
                table: "RelComment");

            migrationBuilder.AlterColumn<int>(
                name: "RELQ_ID",
                table: "RelComment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK__RelCommen__RELQ___3C69FB99",
                table: "RelComment",
                column: "RELQ_ID",
                principalTable: "RelQuestion",
                principalColumn: "RELQ_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
