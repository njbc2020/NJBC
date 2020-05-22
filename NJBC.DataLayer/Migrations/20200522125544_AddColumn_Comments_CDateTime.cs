using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NJBC.DataLayer.Migrations
{
    public partial class AddColumn_Comments_CDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CDate",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CDate",
                table: "Comments");
        }
    }
}
