using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.DTOs;
using StudentManagement.Core.Interfaces;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all students.");
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching student with ID {Id}", id);
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound(new { Message = $"Student with ID {id} not found." });

            return Ok(student);
        }

    
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StudentDto studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Adding new student: {Name}", studentDto.Name);
            await _studentService.AddStudentAsync(studentDto);
            return Ok(new { Message = "Student added successfully." });
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentDto studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Updating student with ID {Id}", id);
            await _studentService.UpdateStudentAsync(id, studentDto);
            return Ok(new { Message = "Student updated successfully." });
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting student with ID {Id}", id);
            await _studentService.DeleteStudentAsync(id);
            return Ok(new { Message = "Student deleted successfully." });
        }
    }
}
