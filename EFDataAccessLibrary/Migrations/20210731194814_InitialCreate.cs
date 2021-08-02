using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataAccessLibrary.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsContent",
                columns: table => new
                {
                    NewsContentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsContent", x => x.NewsContentID);
                });

            migrationBuilder.CreateTable(
                name: "PlayerPosition",
                columns: table => new
                {
                    PositionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPosition", x => x.PositionID);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamID);
                });

            migrationBuilder.CreateTable(
                name: "NewsHeader",
                columns: table => new
                {
                    NewsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewsPublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    NewsContentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsHeader", x => x.NewsId);
                    table.ForeignKey(
                        name: "FK_NewsHeader_NewsContent_NewsContentID",
                        column: x => x.NewsContentID,
                        principalTable: "NewsContent",
                        principalColumn: "NewsContentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PlayerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    NickName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Prize = table.Column<double>(type: "float", nullable: false),
                    Potencial = table.Column<int>(type: "int", nullable: false),
                    Aim = table.Column<int>(type: "int", nullable: false),
                    Knowledge = table.Column<int>(type: "int", nullable: false),
                    PlayerLevel = table.Column<int>(type: "int", nullable: false),
                    PositionID = table.Column<int>(type: "int", nullable: false),
                    PlayerPositionPositionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PlayerID);
                    table.ForeignKey(
                        name: "FK_Player_PlayerPosition_PlayerPositionPositionID",
                        column: x => x.PlayerPositionPositionID,
                        principalTable: "PlayerPosition",
                        principalColumn: "PositionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTeam",
                columns: table => new
                {
                    PlayerID = table.Column<int>(type: "int", nullable: false),
                    TeamID = table.Column<int>(type: "int", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExitDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTeam", x => new { x.PlayerID, x.TeamID });
                    table.ForeignKey(
                        name: "FK_PlayerTeam_Player_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Player",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerTeam_Team_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Team",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsHeader_NewsContentID",
                table: "NewsHeader",
                column: "NewsContentID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_PlayerPositionPositionID",
                table: "Player",
                column: "PlayerPositionPositionID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTeam_TeamID",
                table: "PlayerTeam",
                column: "TeamID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsHeader");

            migrationBuilder.DropTable(
                name: "PlayerTeam");

            migrationBuilder.DropTable(
                name: "NewsContent");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "PlayerPosition");
        }
    }
}
