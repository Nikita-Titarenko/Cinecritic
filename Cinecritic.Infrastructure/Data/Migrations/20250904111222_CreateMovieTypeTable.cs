using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinecritic.Migrations
{
    /// <inheritdoc />
    public partial class CreateMovieTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieTypeId",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MovieTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MovieTypeId",
                table: "Movies",
                column: "MovieTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_MovieTypes_MovieTypeId",
                table: "Movies",
                column: "MovieTypeId",
                principalTable: "MovieTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_MovieTypes_MovieTypeId",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "MovieTypes");

            migrationBuilder.DropIndex(
                name: "IX_Movies_MovieTypeId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MovieTypeId",
                table: "Movies");
        }
    }
}
