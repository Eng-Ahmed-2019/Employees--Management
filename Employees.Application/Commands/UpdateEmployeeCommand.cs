using MediatR;
using Employees.Application.DTOs;

namespace Employees.Application.Commands
{
    public class UpdateEmployeeCommand : IRequest<EmployeeDto>, IBaseRequest
    {
        public int EmployeeId { get; }
        public EmployeeCreateDto? Employee { get; }

        public UpdateEmployeeCommand(int employeeId, EmployeeCreateDto? employee)
        {
            EmployeeId = employeeId;
            Employee = employee;
        }
    }
}