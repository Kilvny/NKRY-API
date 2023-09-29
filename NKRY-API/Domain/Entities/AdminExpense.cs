using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.Domain.Entities
{
    public class AdminExpense : Expense
    {
        public Guid AdminId { get; set; }
        public User? Admin { get; set; }
    }

}