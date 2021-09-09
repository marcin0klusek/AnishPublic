using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSky.Data.Migrations
{
    public partial class AddIdentityUserCurrenciesProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Gold",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
               name: "Dollars",
               table: "AspNetUsers",
               type: "int",
               nullable: false,
               defaultValue: 0);
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
