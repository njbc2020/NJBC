using Microsoft.EntityFrameworkCore.Migrations;

namespace NJBC.DataLayer.Migrations
{
    public partial class ForeignKey_Mandetory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__RelQuesti__ORGQ___398D8EEE",
                table: "RelQuestion");

            migrationBuilder.AlterColumn<int>(
                name: "ORGQ_ID",
                table: "RelQuestion",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__RelQuesti__ORGQ___398D8EEE",
                table: "RelQuestion",
                column: "ORGQ_ID",
                principalTable: "OrgQuestion",
                principalColumn: "ORGQ_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__RelQuesti__ORGQ___398D8EEE",
                table: "RelQuestion");

            migrationBuilder.AlterColumn<int>(
                name: "ORGQ_ID",
                table: "RelQuestion",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK__RelQuesti__ORGQ___398D8EEE",
                table: "RelQuestion",
                column: "ORGQ_ID",
                principalTable: "OrgQuestion",
                principalColumn: "ORGQ_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
