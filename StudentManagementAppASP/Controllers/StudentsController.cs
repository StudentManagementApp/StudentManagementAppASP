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

        public StudentsController(IStudentRepository students, IMapper mapper)
        {
            _students = students;
            _mapper = mapper;
        }

        // ✅ GET: api/students
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var list = await _students.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<StudentDto>>(list);
            return Ok(dtoList);
        }

        // ✅ GET: api/students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _students.GetByIdAsync(id);

            if (student == null)
                return NotFound();

            var dto = _mapper.Map<StudentDto>(student);
            return Ok(dto);
        }

        // ✅ POST: api/students
        [HttpPost]
        public async Task<IActionResult> PostStudent(CreateStudentDto dto)
        {
            var student = _mapper.Map<Student>(dto);
            await _students.AddAsync(student);
            var resultDto = _mapper.Map<StudentDto>(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, resultDto);
        }

        // ✅ PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, UpdateStudentDto dto)
        {
            var existingStudent = await _students.GetByIdAsync(id);
            if (existingStudent == null)
                return NotFound();

            _mapper.Map(dto, existingStudent); // update fields
            var updated = await _students.UpdateAsync(existingStudent);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // ✅ DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var ok = await _students.DeleteAsync(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}
