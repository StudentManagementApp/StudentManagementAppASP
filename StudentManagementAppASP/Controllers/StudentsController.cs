using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Application.DTOs;

namespace StudentManagementAppASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _students;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentRepository students, IMapper mapper, ILogger<StudentsController> logger)
        {
            _students = students;
            _mapper = mapper;
            _logger = logger;
        }

        // ✅ GET: api/students
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            _logger.LogInformation("Fetching all students at {Time}", DateTime.UtcNow);

            var list = await _students.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<StudentDto>>(list);

            _logger.LogInformation("Successfully fetched {Count} students", dtoList.Count());
            return Ok(dtoList);
        }

        // ✅ GET: api/students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            _logger.LogDebug("Fetching student with ID {Id}", id);

            var student = await _students.GetByIdAsync(id);

            if (student == null)
            {
                _logger.LogWarning("Student with ID {Id} not found at {Time}", id, DateTime.UtcNow);
                return NotFound();
            }

            var dto = _mapper.Map<StudentDto>(student);
            _logger.LogInformation("Returning student {Id}: {Name}", id, dto.FirstName);
            return Ok(dto);
        }

        // ✅ POST: api/students
        [HttpPost]
        public async Task<IActionResult> PostStudent(CreateStudentDto dto)
        {
            _logger.LogInformation("Adding a new student at {Time}", DateTime.UtcNow);

            var student = _mapper.Map<Student>(dto);
            await _students.AddAsync(student);

            _logger.LogInformation("Student {Name} added successfully with ID {Id}", student.FirstName, student.Id);

            var resultDto = _mapper.Map<StudentDto>(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, resultDto);
        }

        // ✅ PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, UpdateStudentDto dto)
        {
            _logger.LogInformation("Updating student with ID {Id}", id);

            var existingStudent = await _students.GetByIdAsync(id);
            if (existingStudent == null)
            {
                _logger.LogWarning("Cannot update. Student with ID {Id} not found.", id);
                return NotFound();
            }

            _mapper.Map(dto, existingStudent);
            var updated = await _students.UpdateAsync(existingStudent);

            if (!updated)
            {
                _logger.LogError("Failed to update student with ID {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Student with ID {Id} updated successfully.", id);
            return NoContent();
        }

        // ✅ DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            _logger.LogWarning("Attempting to delete student with ID {Id}", id);

            var ok = await _students.DeleteAsync(id);

            if (!ok)
            {
                _logger.LogError("Failed to delete student with ID {Id} - not found", id);
                return NotFound();
            }

            _logger.LogInformation("Student with ID {Id} deleted successfully.", id);
            return NoContent();
        }
    }
}
