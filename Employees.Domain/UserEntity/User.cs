using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.Domain.UserEntity
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
    }
}