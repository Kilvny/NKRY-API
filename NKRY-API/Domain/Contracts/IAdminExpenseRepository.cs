using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKRY_API.Domain.Entities;
using NKRY_API.ResourceParameters;

namespace NKRY_API.Domain.Contracts
{
    public interface IAdminExpenseRepository : IGenericRepository<AdminExpense>
    {
        public IEnumerable<AdminExpense> GetAll(AdminsResourceParameters adminResourceParameters);
    }
}