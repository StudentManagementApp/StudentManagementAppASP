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
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollments;
        private readonly IMapper _mapper;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(IEnrollmentRepository enrollments, IMapper mapper, ILogger<EnrollmentController> logger)
        {
            _enrollments = enrollments;
            _mapper = mapper;
            _logger = logger;
        }

        // ✅ GET: api/enrollment
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all enrollments at {time}", DateTime.UtcNow);
            var enrollments = await _enrollments.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
            return Ok(dtoList);
        }

        // ✅ GET: api/enrollment/{studentId}/{courseId}
        [HttpGet("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> GetEnrollment(int studentId, int courseId)
        {
            _logger.LogInformation("Fetching enrollment for StudentID {studentId}, CourseID {courseId}", studentId, courseId);
            var enrollment = await _enrollments.GetAsync(studentId, courseId);

            if (enrollment == null)
            {
                _logger.LogWarning("Enrollment not found for StudentID {studentId}, CourseID {courseId}", studentId, courseId);
                return NotFound();
            }

            var dto = _mapper.Map<EnrollmentDto>(enrollment);
            return Ok(dto);
        }

        // ✅ POST: api/enrollment
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Invalid enrollment data received at {time}", DateTime.UtcNow);
                return BadRequest("Invalid enrollment data.");
            }

            _logger.LogInformation("Creating enrollment for StudentID {studentId}, CourseID {courseId}", dto.StudentId, dto.CourseId);
            var enrollment = _mapper.Map<Enrollment>(dto);
            await _enrollments.AddAsync(enrollment);

            var result = _mapper.Map<EnrollmentDto>(enrollment);
            _logger.LogInformation("Enrollment created successfully for StudentID {studentId}, CourseID {courseId}", dto.StudentId, dto.CourseId);
            return CreatedAtAction(nameof(GetEnrollment), new { studentId = dto.StudentId, courseId = dto.CourseId }, result);
        }

        // ✅ PUT: api/enrollment/{studentId}/{courseId}
        [HttpPut("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> Update(int studentId, int courseId, [FromBody] UpdateEnrollmentDto dto)
        {
            _logger.LogInformation("Updating enrollment: StudentID {studentId}, CourseID {courseId}", studentId, courseId);
            var enrollment = await _enrollments.GetAsync(studentId, courseId);
            if (enrollment == null)
            {
                _logger.LogWarning("Enrollment not found for StudentID {studentId}, CourseID {courseId}", studentId, courseId);
                return NotFound();
            }

            _mapper.Map(dto, enrollment);
            await _enrollments.UpdateAsync(enrollment);
            _logger.LogInformation("Enrollment updated successfully: StudentID {studentId}, CourseID {courseId}", studentId, courseId);
            return NoContent();
        }

        // ✅ DELETE: api/enrollment/{studentId}/{courseId}
        [HttpDelete("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> Delete(int studentId, int courseId)
        {
            _logger.LogInformation("Attempting to delete enrollment: StudentID {studentId}, CourseID {courseId}", studentId, courseId);
            var existing = await _enrollments.GetAsync(studentId, courseId);
            if (existing == null)
            {
                _logger.LogWarning("Enrollment not found for deletion: StudentID {studentId}, CourseID {courseId}", studentId, courseId);
                return NotFound();
            }

            await _enrollments.DeleteAsync(studentId, courseId);
            _logger.LogInformation("Enrollment deleted successfully: StudentID {studentId}, CourseID {courseId}", studentId, courseId);
            return NoContent();
        }
    }
}
