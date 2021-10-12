using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataAccessLibrary.Migrations
{
    public partial class AddNewsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsHeader_NewsContent_NewsContentID",
                table: "NewsHeader");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "NewsContentID",
                table: "NewsHeader",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "NewsUpdateID",
                table: "NewsHeader",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NewsUpdate",
                columns: table => new
                {
                    UpdateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsUpdate", x => x.UpdateId);
                });

            migrationBuilder.CreateTable(
                name: "Change",
                columns: table => new
                {
                    ChangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsUpdateUpdateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Change", x => x.ChangeId);
                    table.ForeignKey(
                        name: "FK_Change_NewsUpdate_NewsUpdateUpdateId",
                        column: x => x.NewsUpdateUpdateId,
                        principalTable: "NewsUpdate",
                        principalColumn: "UpdateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeElement",
                columns: table => new
                {
                    ChangeElementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeID = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeElement", x => x.ChangeElementID);
                    table.ForeignKey(
                        name: "FK_ChangeElement_Change_ChangeID",
                        column: x => x.ChangeID,
                        principalTable: "Change",
                        principalColumn: "ChangeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsHeader_NewsUpdateID",
                table: "NewsHeader",
                column: "NewsUpdateID");

            migrationBuilder.CreateIndex(
                name: "IX_Change_NewsUpdateUpdateId",
                table: "Change",
                column: "NewsUpdateUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeElement_ChangeID",
                table: "ChangeElement",
                column: "ChangeID");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsHeader_NewsContent_NewsContentID",
                table: "NewsHeader",
                column: "NewsContentID",
                principalTable: "NewsContent",
                principalColumn: "NewsContentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsHeader_NewsUpdate_NewsUpdateID",
                table: "NewsHeader",
                column: "NewsUpdateID",
                principalTable: "NewsUpdate",
                principalColumn: "UpdateId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsHeader_NewsContent_NewsContentID",
                table: "NewsHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsHeader_NewsUpdate_NewsUpdateID",
                table: "NewsHeader");

            migrationBuilder.DropTable(
                name: "ChangeElement");

            migrationBuilder.DropTable(
                name: "Change");

            migrationBuilder.DropTable(
                name: "NewsUpdate");

            migrationBuilder.DropIndex(
                name: "IX_NewsHeader_NewsUpdateID",
                table: "NewsHeader");

            migrationBuilder.DropColumn(
                name: "NewsUpdateID",
                table: "NewsHeader");

            migrationBuilder.AlterColumn<int>(
                name: "NewsContentID",
                table: "NewsHeader",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TRANSACTION_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AMOUNT = table.Column<int>(type: "int", nullable: false),
                    CUSTOMER_EMAIL_ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PAY_REQUEST_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REFERENCE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RESULT_CODE = table.Column<int>(type: "int", nullable: false),
                    RESULT_DESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRANSACTION_STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TRANSACTION_ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_NewsHeader_NewsContent_NewsContentID",
                table: "NewsHeader",
                column: "NewsContentID",
                principalTable: "NewsContent",
                principalColumn: "NewsContentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
