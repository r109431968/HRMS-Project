using HRMS.Application.Features.Employees.Commands;
using HRMS.Application.Features.Employees.Queries;
using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IUnitOfWork unitOfWork, IMediator mediator, ILogger<EmployeesController> logger)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var employees = await _unitOfWork.Employees.GetAllAsync();
        //        return Ok(employees);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while fetching employees.");
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await _mediator.Send(new GetAllEmployeesQuery());
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employees.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //[Authorize]
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    try
        //    {
        //        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        //        if (employee == null)
        //            return NotFound();
        //        return Ok(employee);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while fetching employee with id {Id}.", id);
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
                if (employee == null)
                    return NotFound(new { message = $"Employee with id {id} not found." });
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employee with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] Employee employee)
        //{
        //    if (employee == null)
        //        return BadRequest("Employee data is required.");

        //    try
        //    {
        //        await _unitOfWork.Employees.AddAsync(employee);
        //        await _unitOfWork.SaveChangesAsync();
        //        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while creating employee.");
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
        {
            try
            {
                var employeeId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = employeeId }, employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating employee.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeCommand command)
        {
            try
            {
                command.Id = id;
                var result = await _mediator.Send(command);
                if (!result)
                    return NotFound(new { message = $"Employee with id {id} not found." });
                return Ok(new { message = "Employee updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating employee with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteEmployeeCommand(id));
                if (!result)
                    return NotFound(new { message = $"Employee with id {id} not found." });
                return Ok(new { message = "Employee deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
