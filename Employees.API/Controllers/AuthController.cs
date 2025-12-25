using MediatR;
using Microsoft.AspNetCore.Mvc;
using Employees.Application.DTOs;
using Employees.Application.Queries;
using Employees.Application.Commands;
using Microsoft.AspNetCore.Authorization;

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

        /*
        [HttpGet("download-national-id-pdf/{userId}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DownloadNationalIdPdf(int userId)
        {
            var fileName = await _mediator.Send(
                new GetUserByIdQuery(userId, true)
            );

            if (string.IsNullOrEmpty(fileName))
                return NotFound("File not found");

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                fileName
            );

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not exists on server");

            return PhysicalFile(filePath, "application/pdf", fileName);
        }

        [HttpGet("download-national-id-image/{userId}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DownloadNationalIdImage(int userId)
        {
            var fileName = await _mediator.Send(
                new GetUserByIdQuery(userId, false)
            );

            if (string.IsNullOrEmpty(fileName))
                return NotFound("Image not found");

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                fileName
            );

            if (!System.IO.File.Exists(filePath))
                return NotFound("Image not exists on server");

            var ext = Path.GetExtension(filePath).ToLower();
            var contentType = ext == ".png" ? "image/png" : "image/jpeg";

            return PhysicalFile(filePath, contentType, fileName);
        }
        */

        [HttpGet("download-national-id-files/{userId}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DownloadNationalIdFile(
            int userId,
            [FromQuery] string type)
        {
            bool isPdf;
            string contentType;

            if (type.ToLower().Trim() == "pdf") isPdf = true;
            else if (type.ToLower().Trim() == "image") isPdf = false;
            else return BadRequest("Type must be pdf or image");

            var fileName = await _mediator.Send(
                new GetUserByIdQuery(userId, isPdf)
            );

            if (string.IsNullOrEmpty(fileName)) return NotFound("File not found");

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                fileName
            );

            if (!System.IO.File.Exists(filePath)) return NotFound("File not exists on server");
            if (isPdf) contentType = "application/pdf";
            else
            {
                var ext = Path.GetExtension(filePath).ToLower();
                contentType = ext == ".png" ? "image/png" : "image/jpeg";
            }

            return PhysicalFile(filePath, contentType, fileName);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequestDto request)
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