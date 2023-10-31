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
        public string EmployeeId { get; set; }
        public string? PassportNumber { get; set; }
        public ICollection<EmployeeFinance>? EmployeeFinance { get; set; }
        public Guid? CarId { get; set; }
        public Car? Car { get; set; }
    }
}