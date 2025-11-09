using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;

namespace StudentManagementAppASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courses;

        public CourseController(ICourseRepository courses)
        {
            _courses = courses;
        }

        // GET: api/course
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courses.GetAllAsync();
            return Ok(courses);
        }

        // GET: api/course/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _courses.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // POST: api/course
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            if (course == null)
                return BadRequest("Invalid course data.");

            await _courses.AddAsync(course);
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        // PUT: api/course/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            if (course == null)
                return BadRequest("Course data cannot be null.");

            if (id != course.Id)
                return BadRequest("ID mismatch.");

            await _courses.UpdateAsync(course);
            return NoContent();
        }

        // DELETE: api/course/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _courses.DeleteAsync(id);
            return NoContent();
        }
    }
}
