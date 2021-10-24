using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataAccessLibrary.Migrations
{
    public partial class AddWarningBar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarningBar",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningBar", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarningBar");
        }
    }
}
