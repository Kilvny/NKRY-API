using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NKRY_API.Migrations
{
    /// <inheritdoc />
    public partial class personalDetailsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalDetails_employees_EmployeeId",
                table: "PersonalDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalDetails",
                table: "PersonalDetails");

            migrationBuilder.RenameTable(
                name: "PersonalDetails",
                newName: "personalDetails");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalDetails_EmployeeId",
                table: "personalDetails",
                newName: "IX_personalDetails_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_personalDetails",
                table: "personalDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_personalDetails_employees_EmployeeId",
                table: "personalDetails",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_personalDetails_employees_EmployeeId",
                table: "personalDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_personalDetails",
                table: "personalDetails");

            migrationBuilder.RenameTable(
                name: "personalDetails",
                newName: "PersonalDetails");

            migrationBuilder.RenameIndex(
                name: "IX_personalDetails_EmployeeId",
                table: "PersonalDetails",
                newName: "IX_PersonalDetails_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalDetails",
                table: "PersonalDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalDetails_employees_EmployeeId",
                table: "PersonalDetails",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
