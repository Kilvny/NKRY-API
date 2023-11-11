using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }
        public new IEnumerable<Employee> GetAll()
        {
            List<Employee> employee = _applicationContext
                .employees
                .Include(e => e.MonthlyFinance)
                .ThenInclude(e => e.MonthlyExpnenses)
                .Include(e => e.Car)
                .Include(e => e.PersonalDetails)
                .Include(e => e.FixedFinance)
                .Include(e => e.FixedExpnenses).ToList();
            return employee;
        }

        public new Employee GetById(Guid id)
        {
            Employee employee = _applicationContext.employees
                .Include(e => e.MonthlyFinance)
                .ThenInclude(e => e.MonthlyExpnenses)
                .Include(e => e.Car)
                .Include(e => e.PersonalDetails)
                .Include(e => e.FixedFinance)
                .Include(e => e.FixedExpnenses)
                .Where(e => e.Id == id)
                .FirstOrDefault();

            List<Expense> fixedExpenses = _applicationContext.expenses.Where(ex => ex.EmployeeId == id).Where(ex => ex.IsFixed == true).ToList();
            employee.FixedExpnenses = fixedExpenses;

            return employee;
        }
        public EmployeeFinance GetEmployeeFinanceByYearAndMonth(Guid employeeId, int year, int month)
        {

            var employeeFinances = _applicationContext.employeeFinances as IQueryable<EmployeeFinance>;
            // var employeeFinances = employee.EmployeeFinance as IQueryable<EmployeeFinance>;
            EmployeeFinance filteredEmployeeFinance = employeeFinances
                    .Include(ef => ef.MonthlyExpnenses)
                    .Where(ef => ef.EmployeeId == employeeId)
                    .Where(ef => ef.DueYear == year && ef.DueMonth == month)
                    .FirstOrDefault();

            return filteredEmployeeFinance;
        }

        public Employee GetEmployeeWithAllFinances(Guid employeeId)
        {
            var employee = _applicationContext
                .employees.Where(e => e.Id == employeeId)
                .Include(e => e.MonthlyFinance)
                .ThenInclude(e=> e.MonthlyExpnenses)
                .Include(e => e.FixedFinance)
                .Include(e => e.FixedExpnenses)
                .FirstOrDefault();
            return employee;
        }

        public IEnumerable<Expense> GetEmployeeExpensesByYearAndMonth(Guid employeeId, int year, int month)
        {
            EmployeeFinance employeeFinance = GetEmployeeFinanceByYearAndMonth(employeeId, year, month);
            
            if (employeeFinance != null)
            {
                IEnumerable<Expense> employeeExpenses = employeeFinance.MonthlyExpnenses;
                return employeeExpenses;    
            } 

            return new List<Expense>();

        }

        public IEnumerable<EmployeeFinance> GetAllEmployeeVariableFinance(Guid employeeId)
        {
            var employeeFinances = _applicationContext.employeeFinances as IQueryable<EmployeeFinance>;
            // var employeeFinances = employee.EmployeeFinance as IQueryable<EmployeeFinance>;
            List<EmployeeFinance> employeeFinancesResult = employeeFinances
                    .Where(ef => ef.EmployeeId == employeeId)
                    .Include(ef => ef.MonthlyExpnenses)
                    .ToList();

            return employeeFinancesResult;
        }




    }
}