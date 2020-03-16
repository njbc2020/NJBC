using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NJBC.DataLayer.Migrations
{
    public partial class AddUsesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrgQuestion",
                columns: table => new
                {
                    ORGQ_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ORGQ_ID_Name = table.Column<string>(maxLength: 50, nullable: true),
                    OrgQSubject = table.Column<string>(nullable: true),
                    OrgQBody = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgQuestion", x => x.ORGQ_ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    LastDatetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RelQuestion",
                columns: table => new
                {
                    ORGQ_ID = table.Column<int>(nullable: true),
                    RELQ_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RELQ_ID_Name = table.Column<string>(maxLength: 50, nullable: true),
                    RELQ_RANKING_ORDER = table.Column<string>(maxLength: 50, nullable: true),
                    RELQ_CATEGORY = table.Column<string>(maxLength: 250, nullable: true),
                    RELQ_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    RELQ_USERID = table.Column<string>(maxLength: 150, nullable: true),
                    RELQ_USERNAME = table.Column<string>(maxLength: 150, nullable: true),
                    RELQ_RELEVANCE2ORGQ = table.Column<string>(maxLength: 150, nullable: true),
                    RelQSubject = table.Column<string>(nullable: true),
                    RelQBody = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelQuestion", x => x.RELQ_ID);
                    table.ForeignKey(
                        name: "FK__RelQuesti__ORGQ___398D8EEE",
                        column: x => x.ORGQ_ID,
                        principalTable: "OrgQuestion",
                        principalColumn: "ORGQ_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RelComment",
                columns: table => new
                {
                    RELQ_ID = table.Column<int>(nullable: true),
                    RELC_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RELC_ID_Name = table.Column<string>(maxLength: 50, nullable: true),
                    RELC_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    RELC_USERID = table.Column<string>(maxLength: 50, nullable: true),
                    RELC_USERNAME = table.Column<string>(maxLength: 200, nullable: true),
                    RELC_RELEVANCE2ORGQ = table.Column<string>(maxLength: 50, nullable: true),
                    RELC_RELEVANCE2RELQ = table.Column<string>(maxLength: 50, nullable: true),
                    RelCText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelComment", x => x.RELC_ID);
                    table.ForeignKey(
                        name: "FK__RelCommen__RELQ___3C69FB99",
                        column: x => x.RELQ_ID,
                        principalTable: "RelQuestion",
                        principalColumn: "RELQ_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelComment_RELQ_ID",
                table: "RelComment",
                column: "RELQ_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RelQuestion_ORGQ_ID",
                table: "RelQuestion",
                column: "ORGQ_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelComment");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RelQuestion");

            migrationBuilder.DropTable(
                name: "OrgQuestion");
        }
    }
}
