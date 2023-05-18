using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disasters.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTestFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION [dbo].[TestFunction]
                (
                    @input nvarchar(50)
                )
                RETURNS nvarchar(50)
                AS
                BEGIN
                    RETURN @input
                END
            ");

            migrationBuilder.Sql(@"CREATE VIEW [dbo].[TestView] AS SELECT 1 AS [Id], 'Test' AS [Name]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [dbo].[TestView]");
            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[TestFunction]");
        }
    }
}
