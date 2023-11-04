using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Job { get; set; }
        public string? Nationality { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Photo { get; set; }
        public string? EmployeeIdNumber { get; set; }
        public string? PassportNumber { get; set; }
        public ICollection<EmployeeFinance>? MonthlyFinance { get; set; }
        public Guid? CarId { get; set; }
        public Car? Car { get; set; }
        public PersonalDetails? PersonalDetails { get; set; }
        public FixedFinance? FixedFinance { get; set; }
        public ICollection<Expense>? FixedExpnenses { get; set; }


    }
}