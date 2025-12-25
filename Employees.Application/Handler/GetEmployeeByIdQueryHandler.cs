using MediatR;
using Employees.Application.DTOs;
using Employees.Application.Queries;
using Employees.Infrastructure.Data;

namespace Employees.Application.Handler
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly ApplicationDbContext _dbContext;
        public GetEmployeeByIdQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees.FindAsync(request.Id);
            if (employee == null) 
            {
                return null;
            }

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