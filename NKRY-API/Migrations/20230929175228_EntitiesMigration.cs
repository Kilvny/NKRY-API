using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NKRY_API.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManfactureYear = table.Column<int>(type: "int", nullable: false),
                    PlateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "employeeFinances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveriesMade = table.Column<int>(type: "int", nullable: false),
                    DeliveryRate = table.Column<int>(type: "int", nullable: false),
                    BaseSalary = table.Column<float>(type: "real", nullable: false),
                    TotalSalary = table.Column<float>(type: "real", nullable: true),
                    DueMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeFinances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Items = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeFinanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeFinanceId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_employees_cars_CarId",
                        column: x => x.CarId,
                        principalTable: "cars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_employees_employeeFinances_EmployeeFinanceId1",
                        column: x => x.EmployeeFinanceId1,
                        principalTable: "employeeFinances",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeFinanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdminId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expenses_AspNetUsers_AdminId1",
                        column: x => x.AdminId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_expenses_employeeFinances_EmployeeFinanceId",
                        column: x => x.EmployeeFinanceId,
                        principalTable: "employeeFinances",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceWithVAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BillTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VATRegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommercialRegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QRUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_invoices_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_employees_CarId",
                table: "employees",
                column: "CarId",
                unique: true,
                filter: "[CarId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_employees_EmployeeFinanceId1",
                table: "employees",
                column: "EmployeeFinanceId1");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_AdminId1",
                table: "expenses",
                column: "AdminId1");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_EmployeeFinanceId",
                table: "expenses",
                column: "EmployeeFinanceId");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_OrderId",
                table: "invoices",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "cars");

            migrationBuilder.DropTable(
                name: "employeeFinances");

            migrationBuilder.DropTable(
                name: "orders");
        }
    }
}
