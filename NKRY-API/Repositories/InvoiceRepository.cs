using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.ResourceParameters;

namespace NKRY_API.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            
        }

        public IEnumerable<Invoice> GetAll(InvoicesResourceParameters invoicesResourceParameters)
        {
            var notImplementedYet = new List<Invoice>();
            return notImplementedYet;
        }
    }
}