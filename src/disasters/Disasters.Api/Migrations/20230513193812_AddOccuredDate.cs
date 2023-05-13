using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disasters.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOccuredDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Occured",
                table: "Disasters",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occured",
                table: "Disasters");
        }
    }
}
