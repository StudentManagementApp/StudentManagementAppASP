using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using StudentManagementApp.Application.DTOs;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;

namespace StudentManagementAppASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courses;
        private readonly IMapper _mapper;

        public CourseController(ICourseRepository courses, IMapper mapper)
        {
            _courses = courses;
            _mapper = mapper;
        }

        // ✅ GET: api/course
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courses.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<CourseDto>>(courses);
            return Ok(dtoList);
        }

        // ✅ GET: api/course/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _courses.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            var dto = _mapper.Map<CourseDto>(course);
            return Ok(dto);
        }

        // ✅ POST: api/course
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid course data.");

            var course = _mapper.Map<Course>(dto);
            await _courses.AddAsync(course);

            var result = _mapper.Map<CourseDto>(course);
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, result);
        }

        // ✅ PUT: api/course/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid course data.");

            var course = await _courses.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            _mapper.Map(dto, course);
            await _courses.UpdateAsync(course);

            return NoContent();
        }

        // ✅ DELETE: api/course/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courses.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            await _courses.DeleteAsync(id);
            return NoContent();
        }
    }
}
