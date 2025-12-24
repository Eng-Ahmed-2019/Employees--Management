using System.ComponentModel.DataAnnotations;

namespace Employees.Application.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Please enter your name here")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your password here")]
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters")]
        public string Password { get; set; } = null!;
    }
}