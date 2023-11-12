using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.Domain.Entities
{
    public class EmployeeFinance : Finance
    {
        public Guid Id { get; set; }
        public int? DeliveriesMade { get; set; } = 0;
        // can be changed by the admin only
        public decimal? TotalSalary { get; set; } = 0;
        public int? DueMonth { get; set; }
        public int? DueYear { get; set; }
        public ICollection <Expense>? MonthlyExpenses{ get; set; }

    }
}