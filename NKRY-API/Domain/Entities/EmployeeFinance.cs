using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.Domain.Entities
{
    public class EmployeeFinance
    {
        public Guid Id { get; set; }
        [Required]
        public int DeliveriesMade { get; set; }
        // can be changed by the admin only
        public int DeliveryRate { get; set; }
        [Required]
        public int BaseSalary { get; set; }
        public int? TotalSalary { get; set; }
        public DateOnly? DueMonth { get; set; }
        public Guid EmployeeId { get; set; }
        public ICollection<Expense>? EmployeeExpenses { get; set; }
        
    }
}