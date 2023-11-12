using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Repositories.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private IUserRepository _user;
        private IDepartmentRepository _department;
        private IAdminExpenseRepository _adminExpense;
        private ICarRepository _car;
        private IEmployeeFinanceRepository _employeeFinance;
        private IEmployeeRepository _employee;
        private IExpenseRepository _expense;
        private IInvoiceRepository _invoice;
        private IOrderRepository _order;
        private IGenericRepository<FixedFinance> _finance;
        private IGenericRepository<ExpenseNames> _expenseNames;
        private IGenericRepository<PersonalDetails> _personalDetails;

        public UnitOfWork(ApplicationContext context, ILogger<UnitOfWork> logger, UserManager<User> userManager, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context, _logger, _userManager, _configuration);
                }
                return _user;
            }
        }
        public IDepartmentRepository Department
        {
            get
            {
                if (_department == null)
                {
                    _department = new DepartmentRepository(_context);
                }
                return _department;
            }
        }
        public IAdminExpenseRepository AdminExpense
        {
            get 
            {
                if(_adminExpense == null)
                    _adminExpense = new AdminExpenseRepository(_context);
                return _adminExpense;
            }
        }
        public ICarRepository Car
        {
            get 
            {
                if(_car == null)
                    _car = new CarRepository(_context);
                return _car;
            }
        }

        public IEmployeeFinanceRepository EmployeeFinance
        {
            get
            {
                if(_employeeFinance == null)
                    _employeeFinance = new EmployeeFinanceRepository(_context);
                return _employeeFinance;
            }
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if(_employee == null)
                    _employee = new EmployeeRepository(_context);
                return _employee;
            }
        }

        public IExpenseRepository Expense
        {
            get
            {
                if(_expense == null)
                    _expense = new ExpenseRepository(_context);
                return _expense;
            }
        }
        public IInvoiceRepository Invoice
        {
            get
            {
                if(_invoice == null)
                    _invoice = new InvoiceRepository(_context);
                return _invoice;
            }
        }

        public IOrderRepository Order
        {
            get
            {
                if(_order == null)
                    _order = new OrderRepository(_context);
                return _order;
            }
        }
        public IGenericRepository<FixedFinance> Finance
        {
            get
            {
                if(_finance == null)
                {
                    _finance = new FinanceRepository(_context);
                }
                return _finance;
            }
        }

        public IGenericRepository<ExpenseNames> ExpenseNames
        {
            get
            {
                if (_expenseNames == null)
                {
                    _expenseNames = new ExpenseNamesRepository(_context);
                }
                return _expenseNames;
            }
        } 
        public IGenericRepository<PersonalDetails> PersonalDetails
        {
            get
            {
                if (_personalDetails == null)
                {
                    _personalDetails = new PersonalDetailsRepository(_context);
                }
                return _personalDetails;
            }
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        public async Task<int> Complete()
        {
            var saveResult = await _context.SaveChangesAsync();
            _logger.LogInformation($"Saved {saveResult} to the database successfully!") ;
            return saveResult;
            
        }
        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            _context.Dispose();
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(disposing: false);
        }

    }
}
