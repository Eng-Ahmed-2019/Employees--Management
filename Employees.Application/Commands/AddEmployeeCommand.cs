using MediatR;
using Employees.Application.DTOs;

namespace Employees.Application.Commands
{
    public class AddEmployeeCommand : IRequest<EmployeeDto>, IBaseRequest
    {
        public EmployeeCreateDto? Employee { get; }

        public AddEmployeeCommand(EmployeeCreateDto? employee)
        {
            Employee = employee;
        }
    }
}