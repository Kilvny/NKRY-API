using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace NKRY_API.Domain.Entities
{
    public class FixedFinance : Finance
    {
        public Guid Id { get; set; }
        // can be changed by the admin only
        [Required]
        public decimal? BaseSalary { get; set; }
        public decimal? DeliveryRate { get; set; }
    }
}
