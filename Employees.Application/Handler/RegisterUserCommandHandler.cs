using MediatR;
using Application.Security;
using Employees.Application.DTOs;
using Employees.Domain.UserEntity;
using Employees.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Employees.Application.Commands;
using Employees.Application.Security;

namespace Employees.Application.Handler
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponseDto>
    {
        private const long FileSize = 5 * 1024 * 1042; // 5MB
        private readonly ApplicationDbContext _context;

        public RegisterUserCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var pdf = request.RegisterDto.NationalIdPdfPath;
            var image = request.RegisterDto.NationalIdImgPath;
            var allowedImages = new[] { "image/jpeg", "image/png", "image/jpg" };

            if (pdf.ContentType != "application/pdf")
            {
                return new RegisterResponseDto { Message = "National ID file must be PDF" };
            }
            else
            {
                if (pdf.Length > FileSize)
                {
                    return new RegisterResponseDto { Message = "PDF file size must not exceed 5 MB" };
                }
            }

            if (!allowedImages.Contains(image.ContentType))
            {
                return new RegisterResponseDto { Message = "Invalid image type" };
            }
            else
            {
                if(image.Length > FileSize)
                {
                    return new RegisterResponseDto { Message = "Image size must not exceed 5 MB" };
                }
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var pdfFileName = Guid.NewGuid() + ".pdf";
            var pdfPath = Path.Combine(uploadFolder, pdfFileName);
            using(var stream = new FileStream(pdfPath, FileMode.Create))
            {
                await pdf.CopyToAsync(stream);
            }

            var imageExtension = Path.GetExtension(image.FileName);
            var imageFileName = Guid.NewGuid() + imageExtension;
            var imagePath = Path.Combine(uploadFolder, imageFileName);
            using(var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.NationalId == request.RegisterDto.NationalId);
            if (existingUser != null)
            {
                return new RegisterResponseDto
                {
                    Message = "NAtional ID is already exist"
                };
            }

            string passwordError;
            if (!PasswordValidator.IsStrongPassword(request.RegisterDto.Password, out passwordError))
            {
                return new RegisterResponseDto
                {
                    Message = passwordError
                };
            }

            if (!NationalIdValidator.IsValid(request.RegisterDto.NationalId))
            {
                return new RegisterResponseDto
                {
                    Message = "Invalid national ID"
                };
            }

            var user = new User
            {
                UserName = request.RegisterDto.UserName,
                PasswordHash = PasswordHasher.HashPassword(request.RegisterDto.Password),
                Role = request.RegisterDto.Role,
                NationalId = request.RegisterDto.NationalId,
                NationalIdPdfPath = pdfFileName,
                NationalIdImgPath = imageFileName
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegisterResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role,
                NationalId = user.NationalId,
                Message = "Registered successfully"
            };
        }
    }
}