using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.Domain.EmployeeEntity
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { set; get; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}