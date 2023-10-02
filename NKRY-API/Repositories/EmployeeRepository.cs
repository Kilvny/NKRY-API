using System;
using System.Collections.Generic;
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
        public EmployeeFinance GetEmployeeFinance(Employee employee, int year, int month)
        {

            var employeeFinances = _applicationContext.employeeFinances as IQueryable<EmployeeFinance>;
            // var employeeFinances = employee.EmployeeFinance as IQueryable<EmployeeFinance>;
            EmployeeFinance filteredEmployeeFinance = employeeFinances
                                .Include(ef => ef.EmployeeId == employee.Id)
                                .Where(ef => ef.DueYear == year && ef.DueMonth == month)
                                .FirstOrDefault();

            return filteredEmployeeFinance;
        }

        public Employee GetEmployeeByFinance(Guid employeeId)
        {
            var employee = _applicationContext.employees.Include(e => e.EmployeeFinance)
                                .FirstOrDefault(e => e.Id == employeeId);
            return employee;
        }
    }
}