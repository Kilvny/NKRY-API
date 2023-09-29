using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Repositories
{
    public class EmployeeFinanceRepository : GenericRepository<EmployeeFinance>, IEmployeeFinanceRepository
    {
        public EmployeeFinanceRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            
        }
    }
}