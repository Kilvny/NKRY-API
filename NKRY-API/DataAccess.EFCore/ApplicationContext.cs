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
        protected override async void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           await SeedAdmins(builder);
           await SeedUsers(builder);
        }

        private async Task SeedUsers(ModelBuilder builder)
        {

            // Create a regular user
            var user = new User
            {
                Id = "d3401183-8e64-4e5b-8fbd-8d0f3ede8a75",
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "regularuser@nkry.com",
                NormalizedEmail = "REGULARUSER@NKRY.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = "User"
            };

            var password = "9y1|O7k&8"; 
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, password);
            user.Password = user.PasswordHash;

            builder.Entity<User>().HasData(user);
        }

        private async Task SeedAdmins(ModelBuilder builder)
        {
            var admin = new User
            {
                Id = "fc40d3cd-d322-484d-9593-51dc8c6fab1b",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@nkry-manage.com",
                NormalizedEmail = "ADMIN@NKRY-MANAGE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = "Admin"
            };

            var password = "0H63>?vHD"; 
            var passwordHasher = new PasswordHasher<User>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, password);
            admin.Password = admin.PasswordHash;
            builder.Entity<User>().HasData(admin);
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
