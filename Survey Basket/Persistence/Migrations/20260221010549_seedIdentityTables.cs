using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Survey_Basket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefult", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "026d222e7aee4eeb8e8d98cff2e20dfc", "9e69ab8fe5d04c108f811e8f4efcd82d", false, false, "Admin", "ADMIN" },
                    { "7164cc71f59f4ff1af7e9f0faeaf17d5", "22f7004dd58d404dbb8fee28e644dcb4", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5f779d8e7d954b4dbf8b3dc2fc9e8b8a", 0, "7ddb67e74dad4adf94f464ef1b09d2a2", "admin@survey-basket.com", true, "survey basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAELKiGFRShWV5uhqrNIJtuaNoTup9Ep87diVpwJA76pu/DCSrhwNJaM4JJ3iJRM450A==", null, false, "5CC212E1CB7B4D6F8D6A7520522FB69C", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permission", "polls:read", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 2, "permission", "polls:add", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 3, "permission", "polls:update", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 4, "permission", "polls:delete", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 5, "permission", "questions:read", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 6, "permission", "questions:add", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 7, "permission", "questions:update", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 8, "permission", "users:read", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 9, "permission", "users:add", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 10, "permission", "users:update", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 11, "permission", "roles:read", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 12, "permission", "roles:add", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 13, "permission", "roles:update", "026d222e7aee4eeb8e8d98cff2e20dfc" },
                    { 14, "permission", "results:read", "026d222e7aee4eeb8e8d98cff2e20dfc" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "026d222e7aee4eeb8e8d98cff2e20dfc", "5f779d8e7d954b4dbf8b3dc2fc9e8b8a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7164cc71f59f4ff1af7e9f0faeaf17d5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "026d222e7aee4eeb8e8d98cff2e20dfc", "5f779d8e7d954b4dbf8b3dc2fc9e8b8a" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "026d222e7aee4eeb8e8d98cff2e20dfc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5f779d8e7d954b4dbf8b3dc2fc9e8b8a");
        }
    }
}
