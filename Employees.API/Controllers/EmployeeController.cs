using MediatR;
using Microsoft.AspNetCore.Mvc;
using Employees.Application.DTOs;
using Employees.Application.Queries;
using Employees.Application.Commands;
using Microsoft.AspNetCore.Authorization;

namespace Employees.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("autocomplete")]
        [Authorize(Roles = "Viewer,HR,Admin")]
        public async Task<IActionResult> AutoComplete(string term)
        {
            var result = await _mediator.Send(new AutoCompleteEmployeesQuery(term));
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Viewer,HR,Admin")]
        public async Task<IActionResult> GetAllEmployees(string? search, int page = 1, int pageSize = 10)
        {
            bool isLoggedIn = User?.Identity?.IsAuthenticated ?? false;
            var result = await _mediator.Send(new GetAllEmployeesQuery(isLoggedIn, search, page, pageSize));
            if (result == null || !result.Any()) return NotFound("No employees found.");
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
            if (employee == null) return NotFound(new { Message = "Employee not found" });
            return Ok(employee);
        }

        [HttpPost]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto employeeCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var r = await _mediator.Send(new AddEmployeeCommand(employeeCreateDTO));
            if (r == null) return BadRequest("This email if already exists");
            return Ok("Employee created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeCreateDto employeeUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var r = await _mediator.Send(new UpdateEmployeeCommand(id, employeeUpdateDTO));
            if (r == null) return NotFound($"Not Found Employee Match with: \"{id}\"");
            return Ok("Employee updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _mediator.Send(new DeleteEmployeeCommand(id));
            if (!result) return NotFound(new { Message = "Employee not found" });
            return Ok(new { Message = "Employee deleted successfully" });
        }
    }
}