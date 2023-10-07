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

    }


    public static  class TaxInvoiceUrlGenerator
    {
        public static string Generate(string clientUrl, string invoiceNumber)
        {

            string url = ($"{clientUrl}/nkry-ca/customer-service/invoices/{invoiceNumber}");

            return url;
        }
    }
}
