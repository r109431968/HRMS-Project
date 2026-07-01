using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IUnitOfWork unitOfWork, ILogger<DepartmentsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching departments.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Department department)
        {
            if (department == null)
                return BadRequest("Department data is required.");

            try
            {
                await _unitOfWork.Departments.AddAsync(department);
                await _unitOfWork.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAll), new { id = department.Id }, department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating department.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
