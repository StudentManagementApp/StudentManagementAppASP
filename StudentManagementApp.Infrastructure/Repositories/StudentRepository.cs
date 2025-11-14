using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Infrastructure.Data;

namespace StudentManagementApp.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public async Task<bool> UpdateAsync(Student student)
        {
            if (!await _context.Students.AnyAsync(s => s.Id == student.Id)) 
                return false;

            _context.Students.Update(student);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            return true;
        }
    }
}
