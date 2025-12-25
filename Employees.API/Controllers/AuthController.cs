using MediatR;
using Microsoft.AspNetCore.Mvc;
using Employees.Application.DTOs;
using Employees.Application.Commands;

namespace Employees.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _mediator.Send(new RegisterUserCommand(request));
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (loginRequest == null) return BadRequest("Invalid login request.");
            var result = await _mediator.Send(new LoginUserCommand(loginRequest));
            if (result == null) return Unauthorized("Invalid username or password.");
            return Ok(result);
        }
    }
}