using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NKRY_API.Migrations
{
    /// <inheritdoc />
    public partial class seedingDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "DepartmentId", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "d3401183-8e64-4e5b-8fbd-8d0f3ede8a75", 0, null, "f53b4085-2801-4b48-b062-7f603109d7e3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "User", null, true, null, null, false, null, "REGULARUSER@NKRY.COM", "USER", "AQAAAAEAACcQAAAAEAkF60a6SUWxOiwViI9oNlz136jJT7d5XD8fJe3EZaDWk0Hh2J8x5brGRwZTdyNZyQ==", "AQAAAAEAACcQAAAAEAkF60a6SUWxOiwViI9oNlz136jJT7d5XD8fJe3EZaDWk0Hh2J8x5brGRwZTdyNZyQ==", null, false, "User", "02de6f85-12af-4dbe-b808-bba699e55ab8", false, "user" },
                    { "fc40d3cd-d322-484d-9593-51dc8c6fab1b", 0, null, "64dc8720-78d3-4aab-9dfd-7c45aded3079", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "User", null, true, null, null, false, null, "ADMIN@NKRY-MANAGE.COM", "ADMIN", "AQAAAAEAACcQAAAAEAmwsZcE3brCsjdrioDcgofQl50NNFU3TD29yQ2FikCnW7aSjh7ce8zIe9/gucMlrQ==", "AQAAAAEAACcQAAAAEAmwsZcE3brCsjdrioDcgofQl50NNFU3TD29yQ2FikCnW7aSjh7ce8zIe9/gucMlrQ==", null, false, "Admin", "7695b406-417b-4259-a0df-a6de18b9b2cb", false, "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d3401183-8e64-4e5b-8fbd-8d0f3ede8a75");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fc40d3cd-d322-484d-9593-51dc8c6fab1b");
        }
    }
}
