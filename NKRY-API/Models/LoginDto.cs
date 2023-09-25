using System.ComponentModel.DataAnnotations;

namespace NKRY_API.Models
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
