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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public TeacherController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // ✅ GET: api/teacher
        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            var list = await _uow.Teachers.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<TeacherDto>>(list);
            return Ok(dtoList);
        }

        // ✅ GET: api/teacher/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            var teacher = await _uow.Teachers.GetByIdAsync(id);
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

            await _uow.Teachers.AddAsync(teacher);
            await _uow.SaveChangesAsync(); 

            var result = _mapper.Map<TeacherDto>(teacher);
            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, result);
        }

        // ✅ PUT: api/teacher/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutTeacher(int id, [FromBody] UpdateTeacherDto dto)
        {
            var existing = await _uow.Teachers.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            _mapper.Map(dto, existing);

            var updated = await _uow.Teachers.UpdateAsync(existing);
            await _uow.SaveChangesAsync();

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // ✅ DELETE: api/teacher/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var ok = await _uow.Teachers.DeleteAsync(id);
            if (!ok)
                return NotFound();

            await _uow.SaveChangesAsync();

            return NoContent();
        }
    }
}
