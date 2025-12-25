using MediatR;
using Microsoft.EntityFrameworkCore;
using Employees.Application.Queries;
using Employees.Infrastructure.Data;

namespace Employees.Application.Handler
{
    public class GetNationalIdFileQueryHandler : IRequestHandler<GetUserByIdQuery, string?>
    {
        private readonly ApplicationDbContext _context;

        public GetNationalIdFileQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (user == null) return null;

            return request.IsPdf ? user.NationalIdPdfPath : user.NationalIdImgPath;
        }
    }
}