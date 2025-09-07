using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cinecritic.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialRolesAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a51c8989-2651-49ff-8c93-35edd02e546f", null, "Manager", "MANAGER" },
                    { "ec4742cf-d15e-422f-aaa2-2b9e9fac58f9", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6837ef24-d213-42d5-ba8e-ec5184942e6e", 0, "b15d51ff-e0a7-469e-92b1-0974801c7601", "Mykyta", "nikitatitarenko81@gmail.com", true, false, null, "NIKITATITARENKO81@.GMAIL.COM", "NIKITATITARENKO81@.GMAIL.COM", "AQAAAAIAAYagAAAAENXsnG6ZNO0dUct2z9E9wj4XqLaT5s3H/NBoOr1LKix4yW92Tu4Ky8P+0niQOyEE2A==", null, false, "c01edc56-64d0-4e9c-8890-3cd8b3e2d1d9", false, "nikitatitarenko81@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a51c8989-2651-49ff-8c93-35edd02e546f", "6837ef24-d213-42d5-ba8e-ec5184942e6e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec4742cf-d15e-422f-aaa2-2b9e9fac58f9");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a51c8989-2651-49ff-8c93-35edd02e546f", "6837ef24-d213-42d5-ba8e-ec5184942e6e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a51c8989-2651-49ff-8c93-35edd02e546f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6837ef24-d213-42d5-ba8e-ec5184942e6e");
        }
    }
}
