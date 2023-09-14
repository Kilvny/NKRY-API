using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static NKRY_API.Utilities.Constants;

namespace NKRY_API.Domain.Entities
{
    public class User
    {

        public int Id { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [Required(ErrorMessage ="User Email is required")]
        [MaxLength(100)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "User Password is required")]
        public string? Password { get; set; }
        [EnumDataType(typeof(UserRole),ErrorMessage ="User Role Must be: user or admin")]
        [Required]
        public UserRole Role { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
