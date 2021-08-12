using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class tableaaaaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ocupied",
                table: "RestaurantTable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ocupied",
                table: "RestaurantTable",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
