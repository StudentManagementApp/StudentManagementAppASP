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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public StudentsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // ✅ GET: api/students
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var list = await _uow.Students.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<StudentDto>>(list);
            return Ok(dtoList);
        }

        // ✅ GET: api/students/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _uow.Students.GetByIdAsync(id);

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

            await _uow.Students.AddAsync(student);
            await _uow.SaveChangesAsync();

            var resultDto = _mapper.Map<StudentDto>(student);

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, resultDto);
        }

        // ✅ PUT: api/students/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, UpdateStudentDto dto)
        {
            var existingStudent = await _uow.Students.GetByIdAsync(id);

            if (existingStudent == null)
                return NotFound();

            _mapper.Map(dto, existingStudent);

            var updated = await _uow.Students.UpdateAsync(existingStudent);

            await _uow.SaveChangesAsync();

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // ✅ DELETE: api/students/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var ok = await _uow.Students.DeleteAsync(id);

            if (!ok)
                return NotFound();

            await _uow.SaveChangesAsync();

            return NoContent();
        }
    }
}
