using MediatR;
using ClosedXML.Excel;
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

        [HttpGet("export-excel")]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> ExportEmployeesToExcel(string? search)
        {
            var result = await _mediator.Send(new GetAllEmployeesQuery(true, search, 1, int.MaxValue));
            if (result == null || !result.Any()) return NotFound("No employees found to export.");

            using var workBook = new XLWorkbook();
            var worksheet = workBook.Worksheets.Add("Employees");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Full Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Department";
            worksheet.Cell(1, 5).Value = "Salary";
            worksheet.Cell(1, 6).Value = "Created At";

            int row = 2;
            foreach(var emp in result)
            {
                worksheet.Cell(row, 1).Value = emp.Id;
                worksheet.Cell(row, 2).Value = emp.FullName;
                worksheet.Cell(row, 3).Value = emp.Email;
                worksheet.Cell(row, 4).Value = emp.Department;
                worksheet.Cell(row, 5).Value = emp.Salary;
                worksheet.Cell(row, 6).Value = emp.CreatedAt.ToString("yyyy-MM-dd");
                row++;
            }

            using var stream = new MemoryStream();
            workBook.SaveAs(stream);
            stream.Position = 0;
            return File(
                 stream.ToArray(),
                 "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 "Employees.xlsx"
            );
        }

        [HttpGet]
        [Authorize(Roles = "Viewer,HR,Admin")]
        public async Task<IActionResult> GetAllEmployees(string? search, int page, int pageSize)
        {
            //int pageSize = 10;
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