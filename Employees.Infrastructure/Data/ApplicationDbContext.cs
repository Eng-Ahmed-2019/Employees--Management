using Employees.Domain.UserEntity;
using Microsoft.EntityFrameworkCore;
using Employees.Domain.EmployeeEntity;

namespace Employees.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}