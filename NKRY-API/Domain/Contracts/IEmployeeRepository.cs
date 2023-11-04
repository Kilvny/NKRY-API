using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Domain.Contracts
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public EmployeeFinance GetEmployeeFinanceByYearAndMonth(Guid employeeId, int year, int month);
        public Employee GetEmployeeWithAllFinances(Guid employeeId);
        public IEnumerable<Expense> GetEmployeeExpensesByYearAndMonth(Guid employeeId, int year, int month);
        public IEnumerable<EmployeeFinance> GetAllEmployeeVariableFinance(Guid employeeId);
        
        //public IEnumerable<Expense> AddEmployeeExpense(Guid employeeId, int year, int month);


    }
}