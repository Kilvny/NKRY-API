using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        [Required]
        public string? Items {get; set; }
        [Required]
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal? Cost { get; set; } = 0;
        public string? OrderStatus { get; set; }


    }
}