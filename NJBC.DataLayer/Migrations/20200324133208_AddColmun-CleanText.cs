using Microsoft.EntityFrameworkCore.Migrations;

namespace NJBC.DataLayer.Migrations
{
    public partial class AddColmunCleanText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelCtextClean",
                table: "RelComment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelCtextClean",
                table: "RelComment");
        }
    }
}
