using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NJBC.DataLayer.Migrations
{
    public partial class AddPropertyReject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LabelComplete",
                table: "Questions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LabelCompleteDateTime",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reject",
                table: "Questions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LabelDate",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabelComplete",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LabelCompleteDateTime",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Reject",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LabelDate",
                table: "Comments");
        }
    }
}
