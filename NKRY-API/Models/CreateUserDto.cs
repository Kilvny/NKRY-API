using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using NKRY_API.Utilities;

namespace NKRY_API.Models
{
    public class CreateUserDto
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(100)]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(8, ErrorMessage ="Password must be at least 8 chars long")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Department Id Is Required")]
        public int? DepartmentId { get; set; }
    }
}
