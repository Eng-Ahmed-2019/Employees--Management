namespace Employees.Application.DTOs
{
    public class RegisterResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}