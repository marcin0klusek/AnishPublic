using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataAccessLibrary.Migrations
{
    public partial class AddActiveSquadToTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isOnTransfer",
                table: "PlayerTeam",
                newName: "IsOnTransfer");

            migrationBuilder.AddColumn<bool>(
                name: "IsInActiveRoster",
                table: "PlayerTeam",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInActiveRoster",
                table: "PlayerTeam");

            migrationBuilder.RenameColumn(
                name: "IsOnTransfer",
                table: "PlayerTeam",
                newName: "isOnTransfer");
        }
    }
}
