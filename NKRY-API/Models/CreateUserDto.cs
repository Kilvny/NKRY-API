using System.Security.Cryptography;
using System.Text;
using NKRY_API.Utilities;

namespace NKRY_API.Models
{
    public class CreateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public int? DepartmentId { get; set; }
    }
}
