using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NKRY_API.Domain.Entities;

namespace NKRY_API.DataAccess.EFCore
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
        // TODO: seed the admin details + the fixed expense names maybe
        // TODO: make sure how to configure your asp.net api to accept put and delete operations
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<User> users { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<AdminExpense> adminExpenses { get; set; }
        public DbSet<Car> cars { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<EmployeeFinance> employeeFinances { get; set; } 
        public DbSet<Expense> expenses { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<FixedFinance> finances { get; set; }
        public DbSet<ExpenseNames> expenseNames { get; set; }
        public DbSet<PersonalDetails> personalDetails { get; set; }
    }
}
