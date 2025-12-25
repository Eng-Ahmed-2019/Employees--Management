using MediatR;
using Employees.Infrastructure.Data;
using Employees.Application.Commands;

namespace Employees.Application.Handler
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly ApplicationDbContext _dbContext;
        public DeleteEmployeeCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees.FindAsync(request.Id);
            if (employee == null)
            {
                return false;
            }
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}