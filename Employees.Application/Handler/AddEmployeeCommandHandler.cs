using MediatR;
using Employees.Application.DTOs;
using Employees.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Employees.Application.Commands;
using Employees.Domain.EmployeeEntity;

namespace Employees.Application.Handler
{
    public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, EmployeeDto>
    {
        private readonly ApplicationDbContext _dbContext;

        public AddEmployeeCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _dbContext.Employees
                .AnyAsync(e => e.Email == request.Employee!.Email, cancellationToken);
            if (emailExists)
            {
                return new EmployeeDto
                {
                    Id = 0,
                    FullName = "EmailAlreadyExists",
                    Email = request.Employee?.Email ?? string.Empty,
                    Department = string.Empty,
                    Salary = 0,
                    CreatedAt = DateTime.MinValue
                };
            }

            var employee = new Employee
            {
                FullName = request.Employee!.FullName,
                Email = request.Employee.Email,
                Department = request.Employee.Department,
                Salary = request.Employee.Salary,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new EmployeeDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                Department = employee.Department,
                Salary = employee.Salary,
                CreatedAt = employee.CreatedAt
            };
        }
    }
}