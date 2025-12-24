using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.Application.DTOs
{
    public class EmployeeCreateDto
    {
        [Required]
        [StringLength(100,ErrorMessage ="Full name must be = 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "Position must be = 50 characters")]
        public string Department { set; get; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}