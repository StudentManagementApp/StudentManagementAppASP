using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using StudentManagementApp.Application.DTOs;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace StudentManagementAppASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _teachers;
        private readonly IMapper _mapper;
        private readonly ILogger<TeacherController> _logger;

        public TeacherController(ITeacherRepository teachers, IMapper mapper, ILogger<TeacherController> logger)
        {
            _teachers = teachers;
            _mapper = mapper;
            _logger = logger;
        }

        // ✅ GET: api/teacher
        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            _logger.LogInformation("Fetching all teachers at {time}", DateTime.UtcNow);
            var list = await _teachers.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<TeacherDto>>(list);
            return Ok(dtoList);
        }

        // ✅ GET: api/teacher/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            _logger.LogInformation("Fetching teacher with ID {id}", id);
            var teacher = await _teachers.GetByIdAsync(id);
            if (teacher == null)
            {
                _logger.LogWarning("Teacher with ID {id} not found", id);
                return NotFound();
            }

            var dto = _mapper.Map<TeacherDto>(teacher);
            return Ok(dto);
        }

        // ✅ POST: api/teacher
        [HttpPost]
        public async Task<IActionResult> PostTeacher([FromBody] CreateTeacherDto dto)
        {
            _logger.LogInformation("Creating new teacher: {name}", dto.Name);
            var teacher = _mapper.Map<Teacher>(dto);
            await _teachers.AddAsync(teacher);

            var result = _mapper.Map<TeacherDto>(teacher);
            _logger.LogInformation("Teacher created successfully with ID {id}", teacher.Id);
            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, result);
        }

        // ✅ PUT: api/teacher/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutTeacher(int id, [FromBody] UpdateTeacherDto dto)
        {
            _logger.LogInformation("Updating teacher with ID {id}", id);
            var existing = await _teachers.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Teacher with ID {id} not found for update", id);
                return NotFound();
            }

            _mapper.Map(dto, existing);
            var updated = await _teachers.UpdateAsync(existing);

            if (!updated)
            {
                _logger.LogWarning("Teacher update failed for ID {id}", id);
                return NotFound();
            }

            _logger.LogInformation("Teacher with ID {id} updated successfully", id);
            return NoContent();
        }

        // ✅ DELETE: api/teacher/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            _logger.LogInformation("Attempting to delete teacher with ID {id}", id);
            var ok = await _teachers.DeleteAsync(id);
            if (!ok)
            {
                _logger.LogWarning("Teacher with ID {id} not found for deletion", id);
                return NotFound();
            }

            _logger.LogInformation("Teacher with ID {id} deleted successfully", id);
            return NoContent();
        }
    }
}
