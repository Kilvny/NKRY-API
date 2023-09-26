using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.Domain.Entities
{
    public class AdminExpense
    {
        public Guid Id { get; set; }
        public Guid? ExpenseId { get; set; }
        public Expense? Expense { get; set; }
        public Guid? AdminId { get; set; }
        public User? Admin { get; set; }
    }

}