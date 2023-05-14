using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disasters.Api.Migrations
{
    /// <inheritdoc />
    public partial class ManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disasters_Locations_LocationId",
                table: "Disasters");

            migrationBuilder.DropIndex(
                name: "IX_Disasters_LocationId",
                table: "Disasters");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Disasters");

            migrationBuilder.CreateTable(
                name: "DisasterLocation",
                columns: table => new
                {
                    DisastersDisasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationsLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisasterLocation", x => new { x.DisastersDisasterId, x.LocationsLocationId });
                    table.ForeignKey(
                        name: "FK_DisasterLocation_Disasters_DisastersDisasterId",
                        column: x => x.DisastersDisasterId,
                        principalTable: "Disasters",
                        principalColumn: "DisasterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisasterLocation_Locations_LocationsLocationId",
                        column: x => x.LocationsLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisasterLocation_LocationsLocationId",
                table: "DisasterLocation",
                column: "LocationsLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisasterLocation");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Disasters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Disasters_LocationId",
                table: "Disasters",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disasters_Locations_LocationId",
                table: "Disasters",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
