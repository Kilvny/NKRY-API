using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NKRY_API.Migrations
{
    /// <inheritdoc />
    public partial class employeeupdatesmigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Job",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "employees");
        }
    }
}
