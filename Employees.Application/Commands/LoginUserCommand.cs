using MediatR;
using Employees.Application.DTOs;

namespace Employees.Application.Commands
{
    public class LoginUserCommand:IRequest<LoginResponseDto>
    {
        public LoginRequestDto LoginDto { get; }
        public LoginUserCommand(LoginRequestDto dto)
        {
            LoginDto = dto;
        }
    }
}