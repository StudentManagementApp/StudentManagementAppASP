using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;

namespace StudentManagementAppASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollments;

        public EnrollmentController(IEnrollmentRepository enrollments)
        {
            _enrollments = enrollments;
        }

        // GET: api/enrollment
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _enrollments.GetAllAsync();
            return Ok(enrollments);
        }

        // GET: api/enrollment/{studentId}/{courseId}
        [HttpGet("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> GetEnrollment(int studentId, int courseId)
        {
            var enrollment = await _enrollments.GetAsync(studentId, courseId);

            if (enrollment == null)
                return NotFound();

            return Ok(enrollment);
        }

        // POST: api/enrollment
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
        {
            if (enrollment == null)
                return BadRequest("Invalid enrollment data.");

            await _enrollments.AddAsync(enrollment);

            return CreatedAtAction(
                nameof(GetEnrollment),
                new { studentId = enrollment.StudentId, courseId = enrollment.CourseId },
                enrollment
            );
        }

        // PUT: api/enrollment/{studentId}/{courseId}
        [HttpPut("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> Update(int studentId, int courseId, [FromBody] Enrollment enrollment)
        {
            if (enrollment == null)
                return BadRequest("Enrollment data cannot be null.");

            if (studentId != enrollment.StudentId || courseId != enrollment.CourseId)
                return BadRequest("StudentId and CourseId must match the URL parameters.");

            await _enrollments.UpdateAsync(enrollment);
            return NoContent();
        }

        // DELETE: api/enrollment/{studentId}/{courseId}
        [HttpDelete("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> Delete(int studentId, int courseId)
        {
            var existing = await _enrollments.GetAsync(studentId, courseId);
            if (existing == null)
                return NotFound();

            await _enrollments.DeleteAsync(studentId, courseId);
            return NoContent();
        }
    }
}
