using MediatR;
using Employees.Application.DTOs;

namespace Employees.Application.Commands
{
    public class RegisterUserCommand:IRequest<RegisterResponseDto>
    {
        public RegisterRequestDto RegisterDto { get; }
        public RegisterUserCommand(RegisterRequestDto dto)
        {
            RegisterDto = dto;
        }
    }
}