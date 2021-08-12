using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ocuupied",
                table: "RestaurantTable",
                newName: "Ocupied");

            migrationBuilder.AddColumn<string>(
                name: "TimeOfTheDay",
                table: "RestaurantTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfTheDay",
                table: "RestaurantTable");

            migrationBuilder.RenameColumn(
                name: "Ocupied",
                table: "RestaurantTable",
                newName: "Ocuupied");
        }
    }
}
