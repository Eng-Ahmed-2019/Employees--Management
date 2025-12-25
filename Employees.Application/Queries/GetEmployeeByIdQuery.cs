using MediatR;
using Employees.Application.DTOs;

namespace Employees.Application.Queries
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDto>
    {
        public int Id { get; }

        public GetEmployeeByIdQuery(int id)
        {
            Id = id;
        }
    }
}