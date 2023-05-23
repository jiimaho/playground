using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disasters.Api.Migrations
{
    public partial class ChangeForeignKeysToRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisasterLocations_Disasters_DisasterId",
                table: "DisasterLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_DisasterLocations_Locations_LocationId",
                table: "DisasterLocations");

            migrationBuilder.UpdateData(
                table: "Disasters",
                keyColumn: "DisasterId",
                keyValue: new Guid("694be870-2024-41a5-b08a-5054b431b4c2"),
                column: "Occured",
                value: new DateTimeOffset(new DateTime(2023, 5, 23, 18, 39, 36, 885, DateTimeKind.Unspecified).AddTicks(6420), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                name: "FK_DisasterLocations_Disasters_DisasterId",
                table: "DisasterLocations",
                column: "DisasterId",
                principalTable: "Disasters",
                principalColumn: "DisasterId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DisasterLocations_Locations_LocationId",
                table: "DisasterLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisasterLocations_Disasters_DisasterId",
                table: "DisasterLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_DisasterLocations_Locations_LocationId",
                table: "DisasterLocations");

            migrationBuilder.UpdateData(
                table: "Disasters",
                keyColumn: "DisasterId",
                keyValue: new Guid("694be870-2024-41a5-b08a-5054b431b4c2"),
                column: "Occured",
                value: new DateTimeOffset(new DateTime(2023, 5, 23, 18, 34, 39, 712, DateTimeKind.Unspecified).AddTicks(4990), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                name: "FK_DisasterLocations_Disasters_DisasterId",
                table: "DisasterLocations",
                column: "DisasterId",
                principalTable: "Disasters",
                principalColumn: "DisasterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DisasterLocations_Locations_LocationId",
                table: "DisasterLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
