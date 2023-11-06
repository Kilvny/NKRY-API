using Microsoft.Build.Framework;

namespace NKRY_API.Domain.Entities
{
    public class ExpenseNames
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Monthly { get; set; } = true;
    }
}
