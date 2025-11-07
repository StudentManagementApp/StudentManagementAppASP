using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;

namespace StudentManagementAppASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _students;

        public StudentsController(IStudentRepository students)
        {
            _students = students;
        }

        // GET: api/students
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var list = await _students.GetAllAsync();
            return Ok(list);
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _students.GetByIdAsync(id);

            if (student == null)
                return NotFound();

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> PostStudent(Student student)
        {
            await _students.AddAsync(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
                return BadRequest();

            var updated = await _students.UpdateAsync(student);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/students/5
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
