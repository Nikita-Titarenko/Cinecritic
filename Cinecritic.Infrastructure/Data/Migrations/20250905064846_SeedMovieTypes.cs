using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cinecritic.Migrations
{
    /// <inheritdoc />
    public partial class SeedMovieTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MovieTypes",
                columns: new[] { "Id", "MovieTypeName" },
                values: new object[,]
                {
                    { 1, "Movie" },
                    { 2, "Series" },
                    { 3, "Cartoon" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MovieTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MovieTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MovieTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
