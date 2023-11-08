using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NKRY_API.Domain.Entities
{
    public class Expense
    {
        // TODO: Add المصروفات الشخصية , Admin + AdminExpense + EmployeeFinanceExpense tables will be added accordingly
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public Guid? EmployeeFinanceId { get; set; }
        [JsonIgnore]
        public EmployeeFinance? EmployeeFinance { get; set; }
    }
}