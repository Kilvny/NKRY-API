using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NKRY_API.Migrations
{
    /// <inheritdoc />
    public partial class employeeIdMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "employees");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeIdNumber",
                table: "employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeIdNumber",
                table: "employees");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
