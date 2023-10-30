using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKRY_API.Domain.Entities;
using NKRY_API.ResourceParameters;

namespace NKRY_API.Domain.Contracts
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        public IEnumerable<Invoice> GetAll(InvoicesResourceParameters invoicesResourceParameters);
        public int GetInvoiceCount();
        public Invoice GetByInvoiceNumber(string invoiceNumber);

    }
}