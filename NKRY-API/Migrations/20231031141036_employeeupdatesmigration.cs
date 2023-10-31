using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NKRY_API.Migrations
{
    /// <inheritdoc />
    public partial class employeeupdatesmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "employees");
        }
    }
}
