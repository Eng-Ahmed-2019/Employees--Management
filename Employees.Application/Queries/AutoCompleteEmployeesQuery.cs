using MediatR;

namespace Employees.Application.Queries
{
    public class AutoCompleteEmployeesQuery : IRequest<List<string>>
    {
        public string? SearchTerm { get; }
        public AutoCompleteEmployeesQuery(string? searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}