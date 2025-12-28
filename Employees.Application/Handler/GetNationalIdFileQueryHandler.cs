using MediatR;
using Employees.Domain.UserEntity;
using Microsoft.EntityFrameworkCore;
using Employees.Application.Queries;
using Employees.Infrastructure.Data;

namespace Employees.Application.Handler
{
    public class GetUserByIdFullQueryHandler
        : IRequestHandler<GetUserByIdQuery, User?>
    {
        private readonly ApplicationDbContext _context;

        public GetUserByIdFullQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        }
    }
}