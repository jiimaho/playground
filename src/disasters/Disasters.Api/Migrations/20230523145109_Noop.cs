using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disasters.Api.Migrations
{
    public partial class Noop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Disasters",
                keyColumn: "DisasterId",
                keyValue: new Guid("694be870-2024-41a5-b08a-5054b431b4c2"),
                column: "Occured",
                value: new DateTimeOffset(new DateTime(2023, 5, 23, 16, 51, 9, 222, DateTimeKind.Unspecified).AddTicks(2110), new TimeSpan(0, 2, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Disasters",
                keyColumn: "DisasterId",
                keyValue: new Guid("694be870-2024-41a5-b08a-5054b431b4c2"),
                column: "Occured",
                value: new DateTimeOffset(new DateTime(2023, 5, 23, 16, 39, 53, 985, DateTimeKind.Unspecified).AddTicks(7710), new TimeSpan(0, 2, 0, 0, 0)));
        }
    }
}
