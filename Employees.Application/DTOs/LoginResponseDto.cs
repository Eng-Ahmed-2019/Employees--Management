namespace Employees.Application.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public int ExpiresIn { get; set; }
        public string Role { get; set; } = null!;
    }
}