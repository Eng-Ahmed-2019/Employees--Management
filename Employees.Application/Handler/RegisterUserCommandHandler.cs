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
        private readonly ApplicationDbContext _context;

        public RegisterUserCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
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
                NationalId = request.RegisterDto.NationalId
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