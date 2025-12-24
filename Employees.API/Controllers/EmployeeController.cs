using Microsoft.AspNetCore.Mvc;
using Employees.Application.DTOs;
using Employees.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Employees.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("autocomplete")]
        [Authorize(Roles = "Viewer,HR,Admin")]
        public IActionResult AutoComplete(string term)
        {
            return Ok(_employeeService.AutoComplete(term));
        }

        [HttpGet]
        [Authorize(Roles = "Viewer,HR,Admin")]
        public async Task<IActionResult> GetAllEmployees(string? search, string? department, int page = 1)
        {
            bool isLoggedIn = false;
            if (User != null && User.Identity != null)
            {
                isLoggedIn = User.Identity.IsAuthenticated;
            }
            var employees = await _employeeService.GetAllEmployees(isLoggedIn, search, department, page);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound($"Not found Employee match with \"{id}\"");
            }
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
            await _employeeService.AddEmployee(employeeCreateDTO);
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
            var existingEmployee = await _employeeService.GetEmployeeById(id);
            if (existingEmployee == null)
            {
                return NotFound($"Not found Employee match with \"{id}\"");
            }
            await _employeeService.UpdateEmployee(id, employeeUpdateDTO);
            return Ok("Employee updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var existingEmployee = await _employeeService.GetEmployeeById(id);
            if (existingEmployee == null)
            {
                return NotFound($"Not found Employee match with \"{id}\"");
            }
            await _employeeService.DeleteEmployee(id);
            return Ok("Employee deleted successfully");
        }
    }
}