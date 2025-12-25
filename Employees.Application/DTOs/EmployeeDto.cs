namespace Employees.Application.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { set; get; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}