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
        [Key]
        public Guid Id { get; set; }
        [Key]
        [Required]
        public string? InvoiceNumber { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public DateOnly DueDate { get; set; }
        [Required]
        public decimal PriceWithVat { get; set; }
        [Required]
        public string? BillTo { get; set; }
        public int VATRate = 15;
        public string? VATRegistrationNumber { get; set; }
        public string? CommercialRegistrationNo { get; set; }
        public string? QRUrl { get; set; }
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }

         
    }
}