using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinecritic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSetForMovieUserAndWatchList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieUser_AspNetUsers_UserId",
                table: "MovieUser");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieUser_Movies_MovieId",
                table: "MovieUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieUser",
                table: "MovieUser");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a51c8989-2651-49ff-8c93-35edd02e546f", "3fc4ef1a-feae-4e6a-8ee0-ca61cfa47845" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3fc4ef1a-feae-4e6a-8ee0-ca61cfa47845");

            migrationBuilder.RenameTable(
                name: "MovieUser",
                newName: "MovieUsers");

            migrationBuilder.RenameIndex(
                name: "IX_MovieUser_UserId",
                table: "MovieUsers",
                newName: "IX_MovieUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieUsers",
                table: "MovieUsers",
                columns: new[] { "MovieId", "UserId" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "363636b4-141c-4de1-a9be-84b40d93b0b0", 0, "51ed16cd-3c72-4f0d-a084-71d276ff68fb", "Mykyta", "nikitatitarenko81@gmail.com", true, false, null, "NIKITATITARENKO81@GMAIL.COM", "NIKITATITARENKO81@GMAIL.COM", "AQAAAAIAAYagAAAAEPfQ3ski4R69vNWTA6aGW/GuUg+jS4Pi0qobcI5eHRqriWwzJkbPf6gSyKu9/udmMA==", null, false, "eac2da9a-07a6-4ef2-8831-714d58be864c", false, "nikitatitarenko81@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a51c8989-2651-49ff-8c93-35edd02e546f", "363636b4-141c-4de1-a9be-84b40d93b0b0" });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieUsers_AspNetUsers_UserId",
                table: "MovieUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieUsers_Movies_MovieId",
                table: "MovieUsers",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieUsers_AspNetUsers_UserId",
                table: "MovieUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieUsers_Movies_MovieId",
                table: "MovieUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieUsers",
                table: "MovieUsers");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a51c8989-2651-49ff-8c93-35edd02e546f", "363636b4-141c-4de1-a9be-84b40d93b0b0" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "363636b4-141c-4de1-a9be-84b40d93b0b0");

            migrationBuilder.RenameTable(
                name: "MovieUsers",
                newName: "MovieUser");

            migrationBuilder.RenameIndex(
                name: "IX_MovieUsers_UserId",
                table: "MovieUser",
                newName: "IX_MovieUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieUser",
                table: "MovieUser",
                columns: new[] { "MovieId", "UserId" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3fc4ef1a-feae-4e6a-8ee0-ca61cfa47845", 0, "1f60db1c-a5fb-4a28-922b-8739faf5c124", "Mykyta", "nikitatitarenko81@gmail.com", true, false, null, "NIKITATITARENKO81@GMAIL.COM", "NIKITATITARENKO81@GMAIL.COM", "AQAAAAIAAYagAAAAED8COI6CyPFfzOEuk1TCyID9qgVCYzkqM+REk1KMowsFSB0X64rw8sU0wTGhVN+7fA==", null, false, "1b1acc1b-c77a-4f49-855d-c40b8fee2c67", false, "nikitatitarenko81@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a51c8989-2651-49ff-8c93-35edd02e546f", "3fc4ef1a-feae-4e6a-8ee0-ca61cfa47845" });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieUser_AspNetUsers_UserId",
                table: "MovieUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieUser_Movies_MovieId",
                table: "MovieUser",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
