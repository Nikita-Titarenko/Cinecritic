using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinecritic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNullPossibleForRateInMovieUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "MovieUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "363636b4-141c-4de1-a9be-84b40d93b0b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9680d3fb-0fdf-400e-9f3a-af99b8f74e9a", "AQAAAAIAAYagAAAAEEOlGkyKNzycOwFxCtkIMwAYBjcTHvy4koP2Y2bQl7ElNvphw6/y2sf+WwLgPHGW/Q==", "2bb9f22c-696f-41b4-a585-7d4d91273e27" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "MovieUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "363636b4-141c-4de1-a9be-84b40d93b0b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51ed16cd-3c72-4f0d-a084-71d276ff68fb", "AQAAAAIAAYagAAAAEPfQ3ski4R69vNWTA6aGW/GuUg+jS4Pi0qobcI5eHRqriWwzJkbPf6gSyKu9/udmMA==", "eac2da9a-07a6-4ef2-8831-714d58be864c" });
        }
    }
}
