using MediatR;

namespace Employees.Application.Commands
{
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public int Id { get; }
        public DeleteEmployeeCommand(int id)
        {
            Id = id;
        }
    }
}