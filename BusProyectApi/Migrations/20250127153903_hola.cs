using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusProyectApi.Migrations
{
    /// <inheritdoc />
    public partial class hola : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_schedules_routes_RouteId",
                table: "schedules");

            migrationBuilder.DropIndex(
                name: "IX_schedules_RouteId",
                table: "schedules");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "schedules",
                newName: "RouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "schedules",
                newName: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_RouteId",
                table: "schedules",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_schedules_routes_RouteId",
                table: "schedules",
                column: "RouteId",
                principalTable: "routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
