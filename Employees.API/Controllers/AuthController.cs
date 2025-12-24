using Microsoft.AspNetCore.Mvc;
using Employees.Application.DTOs;
using Employees.Application.Services;

namespace Employees.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.Register(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (loginRequest == null) return BadRequest("Invalid login request.");
            var result = await _authService.LoginAsync(loginRequest);
            if (result == null) return Unauthorized("Invalid username or password.");
            return Ok(result);
        }
    }
}