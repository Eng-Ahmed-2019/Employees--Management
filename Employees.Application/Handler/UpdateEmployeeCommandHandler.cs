using MediatR;
using Employees.Application.DTOs;
using Employees.Infrastructure.Data;
using Employees.Application.Commands;

namespace Employees.Application.Handler
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        private readonly ApplicationDbContext _dbContext;
        public UpdateEmployeeCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
            {
                throw new InvalidOperationException($"Employee with ID {request.EmployeeId} not found.");
            }
            employee.FullName = request.Employee!.FullName;
            employee.Email = request.Employee.Email;
            employee.Department = request.Employee.Department;
            employee.Salary = request.Employee.Salary;
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