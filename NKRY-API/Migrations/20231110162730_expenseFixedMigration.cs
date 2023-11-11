using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NKRY_API.Migrations
{
    /// <inheritdoc />
    public partial class expenseFixedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                table: "expenses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFixed",
                table: "expenses");
        }
    }
}
