using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            if (invoicesResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(invoicesResourceParameters));
            }
            string invoiceNumber = invoicesResourceParameters.InvoiceNumber;
            string searchQuery = invoicesResourceParameters.SearchQuery;

            bool isInvoiceNumberNull = string.IsNullOrWhiteSpace(invoiceNumber);
            bool isSearchQueryNull = string.IsNullOrWhiteSpace(searchQuery);

            if (isInvoiceNumberNull && isSearchQueryNull)
            {
                return this.GetAll();
            }
            // it's good practice to use differed execution so we cast invoices object as IQueryable<Invoice> type 
            var invoices = _applicationContext.invoices as IQueryable<Invoice>;
            if (!isInvoiceNumberNull)
            {
                invoiceNumber = invoiceNumber.Trim();
                var filteredInvoices = invoices.Where(i => i.InvoiceNumber == invoiceNumber);
                invoices = filteredInvoices;
            }

            if (!isSearchQueryNull)
            {
                searchQuery = searchQuery.Trim();
                var searchResult = invoices.Where(i => i.InvoiceNumber.Contains(searchQuery)
                || i.BillTo.Contains(searchQuery) || i.Order.OrderStatus.Contains(searchQuery));
                invoices = searchResult;
            }
            // now we execute after the filtration done on the query first
            return invoices.ToList();
        }

        public int GetInvoiceCount()
        {
            int count = GetAll().Count();

            if (count < 1 || count == null)
            {
                return 0;
            }
            return count;
        }
        public Invoice GetByInvoiceNumber(string invoiceNumber)
        {
            var invoices = _applicationContext.invoices
                .Include(i => i.Order)
                .Include(navigationPropertyPath: i => i.Order.Size) as IQueryable<Invoice>;

            Invoice invoice = invoices.Where(i => i.InvoiceNumber == invoiceNumber).FirstOrDefault();
            return invoice;
            
        }
    }
}