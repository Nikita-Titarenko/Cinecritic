using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinecritic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreationDateTimeForMovieUserAndWatchList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InWatchListDateTime",
                table: "WatchLists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LikedDateTime",
                table: "MovieUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WatchedDateTime",
                table: "MovieUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InWatchListDateTime",
                table: "WatchLists");

            migrationBuilder.DropColumn(
                name: "LikedDateTime",
                table: "MovieUsers");

            migrationBuilder.DropColumn(
                name: "WatchedDateTime",
                table: "MovieUsers");
        }
    }
}
