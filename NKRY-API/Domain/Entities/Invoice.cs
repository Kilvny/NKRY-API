using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.Domain.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal PriceWithVAT { get; set; }
        [Required]
        public string? BillTo { get; set; }
        public int VATRate = 15;
        public string? VATRegistrationNumber { get; set; } = "310273137700003";
        public string? CommercialRegistrationNo { get; set; }
        public string? QRUrl { get; set; }
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }

         
    }
}