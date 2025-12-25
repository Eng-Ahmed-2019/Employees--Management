using MediatR;
using System.Text;
using System.Security.Claims;
using Employees.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using Employees.Infrastructure.Data;
using Employees.Application.Commands;
using Employees.Application.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseDto?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public LoginUserCommandHandler(ApplicationDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == request.LoginDto.Username);

        if (user == null) return null;

        var hashedPassword = PasswordHasher.HashPassword(request.LoginDto.Password);
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