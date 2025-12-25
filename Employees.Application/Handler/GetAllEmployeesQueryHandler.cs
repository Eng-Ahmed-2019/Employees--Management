using MediatR;
using Employees.Application.DTOs;
using Employees.Infrastructure.Data;
using Employees.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace Employees.Application.Handler
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, List<EmployeeDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAllEmployeesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var searchTerm = request.Search?.Trim();
            var query = _dbContext.Employees.AsNoTracking();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e =>
                    e.FullName.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm) ||
                    e.Department.Contains(searchTerm)
                );
            }
            var employees = await query
                .OrderBy(e => e.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Email = e.Email,
                    Department = e.Department,
                    Salary = e.Salary,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync(cancellationToken);
            return employees;
        }
    }
}