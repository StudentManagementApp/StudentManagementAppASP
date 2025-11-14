using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using StudentManagementApp.Application.DTOs;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;

namespace StudentManagementAppASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public EnrollmentController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // ✅ GET: api/enrollment
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _uow.Enrollments.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
            return Ok(dtoList);
        }

        // ✅ GET: api/enrollment/{studentId}/{courseId}
        [HttpGet("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> GetEnrollment(int studentId, int courseId)
        {
            var enrollment = await _uow.Enrollments.GetAsync(studentId, courseId);

            if (enrollment == null)
                return NotFound();

            var dto = _mapper.Map<EnrollmentDto>(enrollment);
            return Ok(dto);
        }

        // ✅ POST: api/enrollment
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid enrollment data.");

            var enrollment = _mapper.Map<Enrollment>(dto);

            await _uow.Enrollments.AddAsync(enrollment);
            await _uow.SaveChangesAsync(); 

            var result = _mapper.Map<EnrollmentDto>(enrollment);

            return CreatedAtAction(
                nameof(GetEnrollment),
                new { studentId = enrollment.StudentId, courseId = enrollment.CourseId },
                result
            );
        }

        // ✅ PUT: api/enrollment/{studentId}/{courseId}
        [HttpPut("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> Update(int studentId, int courseId, [FromBody] UpdateEnrollmentDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid enrollment data.");

            var enrollment = await _uow.Enrollments.GetAsync(studentId, courseId);
            if (enrollment == null)
                return NotFound();

            _mapper.Map(dto, enrollment);

            await _uow.Enrollments.UpdateAsync(enrollment);
            await _uow.SaveChangesAsync(); 

            return NoContent();
        }

        // ✅ DELETE: api/enrollment/{studentId}/{courseId}
        [HttpDelete("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> Delete(int studentId, int courseId)
        {
            var existing = await _uow.Enrollments.GetAsync(studentId, courseId);
            if (existing == null)
                return NotFound();

            await _uow.Enrollments.DeleteAsync(studentId, courseId);
            await _uow.SaveChangesAsync();

            return NoContent();
        }
    }
}
