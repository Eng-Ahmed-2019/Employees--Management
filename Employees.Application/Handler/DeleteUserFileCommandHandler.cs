using MediatR;
using Employees.Infrastructure.Data;
using Employees.Application.Commands;

namespace Employees.Application.Handler
{
    public class DeleteUserFileCommandHandler : IRequestHandler<UpdateUserFilesCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteUserFileCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(
            UpdateUserFilesCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null) return false;

            bool isPdf = false;
            bool isImg = false;

            if (request.FileName == user.NationalIdPdfPath) isPdf = true;
            else if (request.FileName == user.NationalIdImgPath) isImg = true;
            else return false;

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                request.FileName
            );

            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            if (isPdf) user.NationalIdPdfPath = string.Empty;
            if (isImg) user.NationalIdImgPath = string.Empty;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}