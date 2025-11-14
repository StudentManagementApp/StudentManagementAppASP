using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Infrastructure.Data;

namespace StudentManagementApp.Infrastructure.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _context;

        public TeacherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(int id)
        {
            return await _context.Teachers.FindAsync(id);
        }

        public async Task AddAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
        }

        public async Task<bool> UpdateAsync(Teacher teacher)
        {
            if (!await _context.Teachers.AnyAsync(t => t.Id == teacher.Id)) 
                return false;

            _context.Teachers.Update(teacher);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return false;

            _context.Teachers.Remove(teacher);
            return true;
        }
    }
}
