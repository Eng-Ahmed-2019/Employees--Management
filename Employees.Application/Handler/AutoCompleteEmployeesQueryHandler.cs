using MediatR;
using Employees.Application.Queries;
using Employees.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class AutoCompleteEmployeesQueryHandler : IRequestHandler<AutoCompleteEmployeesQuery, List<string>>
{
    private readonly ApplicationDbContext _dbContext;

    public AutoCompleteEmployeesQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<string>> Handle(AutoCompleteEmployeesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.SearchTerm))
            return new List<string>();

        return await _dbContext.Employees
            .Where(e =>
                e.FullName.Contains(request.SearchTerm) ||
                e.Email.Contains(request.SearchTerm) ||
                e.Department.Contains(request.SearchTerm)
            )
            .Select(e => e.FullName)
            .Distinct()
            .Take(5)
            .ToListAsync();
    }
}