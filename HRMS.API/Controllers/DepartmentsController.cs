using HRMS.Application.Features.Departments.Commands;
using HRMS.Application.Features.Departments.Queries;
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
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IUnitOfWork unitOfWork, IMediator mediator, ILogger<DepartmentsController> logger)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var departments = await _unitOfWork.Departments.GetAllAsync();
        //        return Ok(departments);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while fetching departments.");
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var departments = await _mediator.Send(new GetAllDepartmentsQuery());
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching departments.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var department = await _mediator.Send(new GetDepartmentByIdQuery(id));
                if (department == null)
                    return NotFound(new { message = $"Department with id {id} not found." });
                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching department with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] Department department)
        //{
        //    if (department == null)
        //        return BadRequest("Department data is required.");

        //    try
        //    {
        //        await _unitOfWork.Departments.AddAsync(department);
        //        await _unitOfWork.SaveChangesAsync();
        //        return CreatedAtAction(nameof(GetAll), new { id = department.Id }, department);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while creating department.");
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command)
        {
            try
            {
                var departmentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = departmentId }, departmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating department.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentCommand command)
        {
            try
            {
                command.Id = id;
                var result = await _mediator.Send(command);
                if (!result)
                    return NotFound(new { message = $"Department with id {id} not found." });
                return Ok(new { message = "Department updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating department with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteDepartmentCommand(id));
                if (!result)
                    return NotFound(new { message = $"Department with id {id} not found." });
                return Ok(new { message = "Department deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting department with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
