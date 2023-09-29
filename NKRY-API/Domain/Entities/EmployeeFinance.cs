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
        public float BaseSalary { get; set; }
        public float? TotalSalary { get; set; }
        public string? DueMonth { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid EmployeeId { get; set; }
        public ICollection<Expense>? EmployeeExpenses { get; set; }
        
    }
}