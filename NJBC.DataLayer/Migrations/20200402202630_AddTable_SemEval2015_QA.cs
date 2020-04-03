using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NJBC.DataLayer.Migrations
{
    public partial class AddTable_SemEval2015_QA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<long>(nullable: false),
                    QID = table.Column<string>(nullable: true),
                    QCATEGORY = table.Column<string>(nullable: true),
                    QDATE = table.Column<DateTime>(nullable: false),
                    QUSERID = table.Column<long>(nullable: false),
                    QTYPE = table.Column<string>(nullable: true),
                    QGOLD_YN = table.Column<string>(nullable: true),
                    QUsername = table.Column<string>(nullable: true),
                    QBody = table.Column<string>(nullable: true),
                    QSubject = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<long>(nullable: false),
                    CID = table.Column<string>(nullable: true),
                    CUSERID = table.Column<long>(nullable: false),
                    CGOLD = table.Column<string>(nullable: true),
                    CGOLD_YN = table.Column<string>(nullable: true),
                    CSubject = table.Column<string>(nullable: true),
                    CBody = table.Column<string>(nullable: true),
                    CBodyClean = table.Column<string>(nullable: true),
                    CUsername = table.Column<string>(nullable: true),
                    ReplayCommentId = table.Column<long>(nullable: true),
                    QuestionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_QuestionId",
                table: "Comments",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId",
                table: "Questions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
