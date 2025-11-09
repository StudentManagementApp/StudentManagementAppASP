using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using StudentManagementApp.Application.DTOs;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;

namespace StudentManagementAppASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _teachers;
        private readonly IMapper _mapper;

        public TeacherController(ITeacherRepository teachers, IMapper mapper)
        {
            _teachers = teachers;
            _mapper = mapper;
        }

        // ✅ GET: api/teacher
        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            var list = await _teachers.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<TeacherDto>>(list);
            return Ok(dtoList);
        }

        // ✅ GET: api/teacher/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            var teacher = await _teachers.GetByIdAsync(id);
            if (teacher == null)
                return NotFound();

            var dto = _mapper.Map<TeacherDto>(teacher);
            return Ok(dto);
        }

        // ✅ POST: api/teacher
        [HttpPost]
        public async Task<IActionResult> PostTeacher([FromBody] CreateTeacherDto dto)
        {
            var teacher = _mapper.Map<Teacher>(dto);
            await _teachers.AddAsync(teacher);

            var result = _mapper.Map<TeacherDto>(teacher);
            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, result);
        }

        // ✅ PUT: api/teacher/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutTeacher(int id, [FromBody] UpdateTeacherDto dto)
        {
            var existing = await _teachers.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            _mapper.Map(dto, existing);
            var updated = await _teachers.UpdateAsync(existing);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // ✅ DELETE: api/teacher/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var ok = await _teachers.DeleteAsync(id);
            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}
