namespace NKRY_API.Models
{
    public class TaxInvoiceDto
    {
        public int Id { get; set; }
        public string? BillTo { get; set; }
        public string? CustomerName { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string? Items { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public int VatRate { get; set; }
        public string? VATRegistrationNumber { get; set; }
        public string? QRUrl { get; set; }
        public decimal PriceWithVAT { get; set; }

    }
}
