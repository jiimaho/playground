using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disasters.Api.Migrations
{
    /// <inheritdoc />
    public partial class DisasterLocations : Migration
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
                name: "DisasterLocations",
                columns: table => new
                {
                    DisasterLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisasterLocations", x => x.DisasterLocationId);
                    table.ForeignKey(
                        name: "FK_DisasterLocations_Disasters_DisasterId",
                        column: x => x.DisasterId,
                        principalTable: "Disasters",
                        principalColumn: "DisasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisasterLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisasterLocations_DisasterId",
                table: "DisasterLocations",
                column: "DisasterId");

            migrationBuilder.CreateIndex(
                name: "IX_DisasterLocations_LocationId",
                table: "DisasterLocations",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisasterLocations");

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
