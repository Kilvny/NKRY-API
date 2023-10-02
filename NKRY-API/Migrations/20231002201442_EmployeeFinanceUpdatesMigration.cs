using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NKRY_API.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeFinanceUpdatesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_employeeFinances_EmployeeFinanceId1",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_EmployeeFinanceId1",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "EmployeeFinanceId",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "EmployeeFinanceId1",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "employeeFinances");

            migrationBuilder.AlterColumn<int>(
                name: "DueMonth",
                table: "employeeFinances",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DeliveryRate",
                table: "employeeFinances",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DueYear",
                table: "employeeFinances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_employeeFinances_EmployeeId",
                table: "employeeFinances",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_employeeFinances_employees_EmployeeId",
                table: "employeeFinances",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employeeFinances_employees_EmployeeId",
                table: "employeeFinances");

            migrationBuilder.DropIndex(
                name: "IX_employeeFinances_EmployeeId",
                table: "employeeFinances");

            migrationBuilder.DropColumn(
                name: "DueYear",
                table: "employeeFinances");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeFinanceId",
                table: "employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeFinanceId1",
                table: "employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DueMonth",
                table: "employeeFinances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryRate",
                table: "employeeFinances",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "employeeFinances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_EmployeeFinanceId1",
                table: "employees",
                column: "EmployeeFinanceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_employees_employeeFinances_EmployeeFinanceId1",
                table: "employees",
                column: "EmployeeFinanceId1",
                principalTable: "employeeFinances",
                principalColumn: "Id");
        }
    }
}
