using System.ComponentModel.DataAnnotations;

namespace NKRY_API.Domain.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        [MaxLength(50, ErrorMessage = "Department Name must not exceed 50 chars")]
        public string? DepartmentName { get; set; }
        public ICollection<User>? DepartmentMembers { get; set; }
    }
}
