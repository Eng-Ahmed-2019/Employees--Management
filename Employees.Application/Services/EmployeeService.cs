using Employees.Application.DTOs;
using Employees.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Employees.Domain.EmployeeEntity;

namespace Employees.Application.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> AutoComplete(string term)
        {
            if (string.IsNullOrEmpty(term))
                return new List<string>();

            return await _dbContext.Employees
                .Where(e =>
                    e.FullName.Contains(term) ||
                    e.Email.Contains(term) ||
                    e.Department.Contains(term)
                )
                .Select(e => e.FullName)
                .Distinct()
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<EmployeeDto>> GetAllEmployees(bool isUserLoggedIn, string? search, string? department, int page = 1, int pageSize = 5)
        {
            var query = _dbContext.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e =>
                    e.FullName.Contains(search) ||
                    e.Email.Contains(search) ||
                    e.Department.Contains(search)
                );
            }

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(e => e.Department == department);
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Email = e.Email,
                    Department = e.Department,
                    Salary = e.Salary,
                    IsActive = isUserLoggedIn ? e.IsActive : false
                })
                .ToListAsync();
        }

        public async Task<EmployeeDto?> GetEmployeeById(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
                return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                Department = employee.Department,
                Salary = employee.Salary,
                IsActive = employee.IsActive
            };
        }

        public async Task AddEmployee(EmployeeCreateDto employeeDto)
        {
            var employee = new Employee
            {
                FullName = employeeDto.FullName,
                Email = employeeDto.Email,
                Department = employeeDto.Department,
                Salary = employeeDto.Salary,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEmployee(int id, EmployeeCreateDto employeeDto)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }
            employee.FullName = employeeDto.FullName;
            employee.Email = employeeDto.Email;
            employee.Department = employeeDto.Department;
            employee.Salary = employeeDto.Salary;
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployee(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }
        }
    }
}