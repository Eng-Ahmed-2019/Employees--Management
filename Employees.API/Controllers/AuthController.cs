using MediatR;
using Microsoft.AspNetCore.Mvc;
using Employees.Application.DTOs;
using Employees.Application.Queries;
using Employees.Application.Commands;
using Employees.Application.Security;
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

        [HttpGet("download-file/{userId}/{fileName}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DownloadFile(int userId, string fileName)
        {
            if (!FileNameValidator.IsValid(fileName)) return BadRequest("Invalid file name");
            var user = await _mediator.Send(new GetUserByIdQuery(userId));
            if (user == null) return NotFound("User not found");
            if (fileName != user.NationalIdPdfPath && fileName != user.NationalIdImgPath) return BadRequest("File does not belong to this user");
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                fileName
            );
            if (!System.IO.File.Exists(filePath)) return NotFound("File not exists on server");
            var ext = Path.GetExtension(filePath).ToLower();
            var contentType = ext switch
            {
                ".pdf" => "application/pdf",
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };
            return PhysicalFile(filePath, contentType, fileName);
        }

        [HttpDelete("delete-file/{userId}/{fileName}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DeleteFile(int userId, string fileName)
        {
            if (!FileNameValidator.IsValid(fileName)) return BadRequest("Invalid file name");
            var result = await _mediator.Send(
                new UpdateUserFilesCommand(userId, fileName)
            );
            if (!result) return BadRequest("Failed to delete file or file not found.");
            return Ok("File deleted successfully.");
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