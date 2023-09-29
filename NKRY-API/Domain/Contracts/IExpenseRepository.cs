using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Domain.Contracts
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        
    }
}