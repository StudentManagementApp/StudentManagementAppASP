using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;

namespace StudentManagementAppASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _teachers;

        public TeacherController(ITeacherRepository teachers)
        {
            _teachers = teachers;
        }

        // GET: api/teacher
        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            var list = await _teachers.GetAllAsync();
            return Ok(list);
        }

        // GET: api/teacher/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            var teacher = await _teachers.GetByIdAsync(id);

            if (teacher == null)
                return NotFound();

            return Ok(teacher);
        }

        // POST: api/teacher
        [HttpPost]
        public async Task<IActionResult> PostTeacher(Teacher teacher)
        {
            await _teachers.AddAsync(teacher);
            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, teacher);
        }

        // PUT: api/teacher/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.Id)
                return BadRequest("Id mismatch");

            var updated = await _teachers.UpdateAsync(teacher);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/teacher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var ok = await _teachers.DeleteAsync(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}
