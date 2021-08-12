using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class tableaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfTheDay",
                table: "RestaurantTable");

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    RestaurantTableId = table.Column<int>(type: "int", nullable: false),
                    TimeOfTheDay = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Occupied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => new { x.ReservationId, x.RestaurantId, x.RestaurantTableId, x.TimeOfTheDay, x.Date, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_Reservation_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_RestaurantTable_RestaurantId_RestaurantTableId",
                        columns: x => new { x.RestaurantId, x.RestaurantTableId },
                        principalTable: "RestaurantTable",
                        principalColumns: new[] { "RestaurantTableId", "RestaurantId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_AppUserId",
                table: "Reservation",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_RestaurantId_RestaurantTableId",
                table: "Reservation",
                columns: new[] { "RestaurantId", "RestaurantTableId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.AddColumn<string>(
                name: "TimeOfTheDay",
                table: "RestaurantTable",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
