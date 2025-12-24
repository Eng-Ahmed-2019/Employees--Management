using System.Text;
using Application.Security;
using System.Security.Claims;
using Employees.Application.DTOs;
using Employees.Domain.UserEntity;
using Employees.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Employees.Application.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Employees.Application.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<RegisterResponseDto> Register(RegisterRequestDto request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (existingUser != null)
                return new RegisterResponseDto
                {
                    Message = "User Name is already exist"
                };

            string passwordError;
            if (!PasswordValidator.IsStrongPassword(request.Password, out passwordError))
            {
                return new RegisterResponseDto
                {
                    Message = passwordError
                };
            }

            if (!NationalIdValidator.IsValid(request.NationalId))
            {
                return new RegisterResponseDto
                {
                    Message = "Invalid national ID"
                };
            }

            DateTime birthDate = NationalIdValidator.GetBirthDate(request.NationalId);
            string gender = NationalIdValidator.GetGender(request.NationalId);

            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                Role = request.Role,
                NationalId = request.NationalId,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegisterResponseDto
            {
                UserName = user.UserName,
                Role = user.Role,
                NationalId = user.NationalId,
                Message = "Registered successfully"
            };
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.Username);
            if (user == null) return null;

            var hashedPassword = PasswordHasher.HashPassword(request.Password);
            if (user.PasswordHash != hashedPassword)
                return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = 60 * 60,
                Role = user.Role
            };
        }
    }
}