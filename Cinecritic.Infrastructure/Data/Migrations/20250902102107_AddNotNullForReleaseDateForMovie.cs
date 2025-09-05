using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinecritic.Migrations
{
    /// <inheritdoc />
    public partial class AddNotNullForReleaseDateForMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
    name: "ReleaseDate",
    table: "Movies",
    type: "DATETIME2",
    nullable: true
    );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
