using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Infrastructure.Data;

namespace StudentManagementApp.Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _db;

        public EnrollmentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _db.Enrollments.ToListAsync();
        }

        public async Task<Enrollment?> GetAsync(int studentId, int courseId)
        {
            return await _db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task AddAsync(Enrollment enrollment)
        {
            await _db.Enrollments.AddAsync(enrollment);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            _db.Enrollments.Update(enrollment);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int studentId, int courseId)
        {
            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment != null)
            {
                _db.Enrollments.Remove(enrollment);
                await _db.SaveChangesAsync();
            }
        }
    }
}
