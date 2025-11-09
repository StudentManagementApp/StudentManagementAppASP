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
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courses;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseRepository courses, IMapper mapper, ILogger<CourseController> logger)
        {
            _courses = courses;
            _mapper = mapper;
            _logger = logger;
        }

        // ✅ GET: api/course
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            _logger.LogInformation("Fetching all courses at {time}", DateTime.UtcNow);
            var courses = await _courses.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<CourseDto>>(courses);
            return Ok(dtoList);
        }

        // ✅ GET: api/course/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            _logger.LogInformation("Fetching course with ID {id}", id);
            var course = await _courses.GetByIdAsync(id);
            if (course == null)
            {
                _logger.LogWarning("Course with ID {id} not found", id);
                return NotFound();
            }

            var dto = _mapper.Map<CourseDto>(course);
            return Ok(dto);
        }

        // ✅ POST: api/course
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Invalid course data received at {time}", DateTime.UtcNow);
                return BadRequest("Invalid course data.");
            }

            _logger.LogInformation("Creating new course: {title}", dto.Title);
            var course = _mapper.Map<Course>(dto);
            await _courses.AddAsync(course);

            var result = _mapper.Map<CourseDto>(course);
            _logger.LogInformation("Course created successfully with ID {id}", course.Id);
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, result);
        }

        // ✅ PUT: api/course/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
        {
            _logger.LogInformation("Updating course with ID {id}", id);
            var course = await _courses.GetByIdAsync(id);
            if (course == null)
            {
                _logger.LogWarning("Course with ID {id} not found", id);
                return NotFound();
            }

            _mapper.Map(dto, course);
            await _courses.UpdateAsync(course);
            _logger.LogInformation("Course with ID {id} updated successfully", id);
            return NoContent();
        }

        // ✅ DELETE: api/course/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            _logger.LogInformation("Attempting to delete course with ID {id}", id);
            var course = await _courses.GetByIdAsync(id);
            if (course == null)
            {
                _logger.LogWarning("Course with ID {id} not found for deletion", id);
                return NotFound();
            }

            await _courses.DeleteAsync(id);
            _logger.LogInformation("Course with ID {id} deleted successfully", id);
            return NoContent();
        }
    }
}
