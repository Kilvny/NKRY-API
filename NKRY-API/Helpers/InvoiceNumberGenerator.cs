using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;

namespace NKRY_API.Helpers
{
    public static class InvoiceNumberGenerator
    {
        public static string Generate(int existingInvoiceCount)
        {
            string invoiceNumber = $"inv-{existingInvoiceCount + 1:D4}";

            return invoiceNumber;
        }

        public static int GetInvoiceCount(ApplicationContext applicationContext)
        {
            return applicationContext.invoices.Count();
        }
    }
}
