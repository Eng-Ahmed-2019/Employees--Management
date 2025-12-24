using System.ComponentModel.DataAnnotations;

namespace Employees.Application.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "User name required and must be = 100 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password required and must be between 6 and 100 characters")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        [StringLength(14, ErrorMessage = "National ID required and must be = 14 characters")]
        public string NationalId { get; set; } = string.Empty;
    }
}