using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disasters.Api.Migrations
{
    public partial class AddAuditTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KeyValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Disasters",
                columns: new[] { "DisasterId", "Occured", "Summary" },
                values: new object[] { new Guid("694be870-2024-41a5-b08a-5054b431b4c2"), new DateTimeOffset(new DateTime(2023, 5, 22, 11, 29, 15, 541, DateTimeKind.Unspecified).AddTicks(6710), new TimeSpan(0, 2, 0, 0, 0)), "Seed" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Country" },
                values: new object[] { new Guid("49c6030e-9dd8-4155-8f65-47394943b804"), "Sweden" });

            migrationBuilder.InsertData(
                table: "DisasterLocations",
                columns: new[] { "DisasterLocationId", "DisasterId", "LocationId" },
                values: new object[] { new Guid("8feb0a12-5317-490c-a4a4-3d8d5c1328c8"), new Guid("694be870-2024-41a5-b08a-5054b431b4c2"), new Guid("49c6030e-9dd8-4155-8f65-47394943b804") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DeleteData(
                table: "DisasterLocations",
                keyColumn: "DisasterLocationId",
                keyValue: new Guid("8feb0a12-5317-490c-a4a4-3d8d5c1328c8"));

            migrationBuilder.DeleteData(
                table: "Disasters",
                keyColumn: "DisasterId",
                keyValue: new Guid("694be870-2024-41a5-b08a-5054b431b4c2"));

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: new Guid("49c6030e-9dd8-4155-8f65-47394943b804"));
        }
    }
}
