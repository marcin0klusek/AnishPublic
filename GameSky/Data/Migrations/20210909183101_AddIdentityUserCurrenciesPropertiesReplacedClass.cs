using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSky.Data.Migrations
{
    public partial class AddIdentityUserCurrenciesPropertiesReplacedClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Dollars",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Dollars",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Gold",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dollars",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "AspNetUsers");
        }
    }
}
