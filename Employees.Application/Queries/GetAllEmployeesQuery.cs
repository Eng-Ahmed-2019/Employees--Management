using MediatR;
using Employees.Application.DTOs;

namespace Employees.Application.Queries
{
    public class GetAllEmployeesQuery : IRequest<List<EmployeeDto>>, IBaseRequest
    {
        public bool IsUserLoggedIn { get; }
        public string? Search { get; }
        public int Page { get; }
        public int PageSize { get; }

        public GetAllEmployeesQuery(bool isUserLoggedIn, string? search, int page, int pageSize)
        {
            IsUserLoggedIn = isUserLoggedIn;
            Search = search;
            Page = page;
            PageSize = pageSize;
        }
    }
}