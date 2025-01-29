using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusProyectApi.Migrations
{
    /// <inheritdoc />
    public partial class AddString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_schedules_buses_BusId",
                table: "schedules"
                );
            migrationBuilder.DropPrimaryKey(
                name: "PK_buses",
                table: "buses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "buses");

            migrationBuilder.AlterColumn<string>(
                name: "BusId",
                table: "schedules",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "BusPlate",
                table: "buses",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_buses",
                table: "buses",
                column: "BusPlate");
            migrationBuilder.AddForeignKey(
                 name: "FK_schedules_buses_BusId",
                 table: "schedules",
                 column: "BusId",
                 principalTable: "buses",
                 principalColumn: "BusPlate",
                 onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                  name: "FK_schedules_routes_RouteId",
                  table: "schedules",
                  column: "RouteId",
                  principalTable: "routes",
                  principalColumn: "Id",
                  onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_buses",
                table: "buses");

            migrationBuilder.DropColumn(
                name: "BusPlate",
                table: "buses");

            migrationBuilder.AlterColumn<Guid>(
                name: "BusId",
                table: "schedules",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(18)",
                oldMaxLength: 18);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "buses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_buses",
                table: "buses",
                column: "Id");
        }
    }
}
