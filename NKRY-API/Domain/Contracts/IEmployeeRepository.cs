using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Domain.Contracts
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public EmployeeFinance GetEmployeeFinance(Guid employeeId, int year, int month);
        public Employee GetEmployeeByFinance(Guid employeeId);
        public IEnumerable<Expense> GetEmployeeExpenses(Guid employeeId, int year, int month);
        //public IEnumerable<Expense> AddEmployeeExpense(Guid employeeId, int year, int month);


    }
}