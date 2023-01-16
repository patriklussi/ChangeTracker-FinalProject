using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectBackend.Migrations
{
    public partial class newTablesForWeeklyMSGandDailyMSG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyMsgs",
                columns: table => new
                {
                    DailyMsgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfWriting = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChallengeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMsgs", x => x.DailyMsgId);
                    table.ForeignKey(
                        name: "FK_DailyMsgs_Challenge_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenge",
                        principalColumn: "ChallengeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyMsgs",
                columns: table => new
                {
                    WeeklyMsgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfWriting = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeeklyMsgDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false),
                    ChallengeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyMsgs", x => x.WeeklyMsgId);
                    table.ForeignKey(
                        name: "FK_WeeklyMsgs_Challenge_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenge",
                        principalColumn: "ChallengeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMsgs_ChallengeId",
                table: "DailyMsgs",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyMsgs_ChallengeId",
                table: "WeeklyMsgs",
                column: "ChallengeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMsgs");

            migrationBuilder.DropTable(
                name: "WeeklyMsgs");
        }
    }
}
