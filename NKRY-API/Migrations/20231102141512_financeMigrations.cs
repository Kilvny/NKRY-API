using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NKRY_API.Migrations
{
    /// <inheritdoc />
    public partial class financeMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employeeFinances_employees_EmployeeId",
                table: "employeeFinances");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "expenses");

            migrationBuilder.DropColumn(
                name: "BaseSalary",
                table: "employeeFinances");

            migrationBuilder.DropColumn(
                name: "DeliveryRate",
                table: "employeeFinances");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "employeeFinances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeliveriesMade",
                table: "employeeFinances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "finances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveryRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_finances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_finances_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisaExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlightTicketsDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DuesPayDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalDetails_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_expenses_EmployeeId",
                table: "expenses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_finances_EmployeeId",
                table: "finances",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetails_EmployeeId",
                table: "PersonalDetails",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_employeeFinances_employees_EmployeeId",
                table: "employeeFinances",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_employees_EmployeeId",
                table: "expenses",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employeeFinances_employees_EmployeeId",
                table: "employeeFinances");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_employees_EmployeeId",
                table: "expenses");

            migrationBuilder.DropTable(
                name: "finances");

            migrationBuilder.DropTable(
                name: "PersonalDetails");

            migrationBuilder.DropIndex(
                name: "IX_expenses_EmployeeId",
                table: "expenses");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "expenses");

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "expenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "employeeFinances",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveriesMade",
                table: "employeeFinances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseSalary",
                table: "employeeFinances",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryRate",
                table: "employeeFinances",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_employeeFinances_employees_EmployeeId",
                table: "employeeFinances",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "Id");
        }
    }
}
