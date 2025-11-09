using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Infrastructure.Data;
using StudentManagementApp.Application.Common.Interfaces;

namespace StudentManagementApp.Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _db;
        private readonly IAppLogger<EnrollmentRepository> _logger;

        public EnrollmentRepository(AppDbContext db, IAppLogger<EnrollmentRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all enrollments from database...");
            var enrollments = await _db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();

            _logger.LogInformation("Fetched {Count} enrollments from database", enrollments.Count);
            return enrollments;
        }

        public async Task<Enrollment?> GetAsync(int studentId, int courseId)
        {
            _logger.LogInformation("Fetching enrollment for StudentID {StudentId} and CourseID {CourseId}", studentId, courseId);

            var enrollment = await _db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null)
                _logger.LogWarning("Enrollment not found for StudentID {StudentId} and CourseID {CourseId}", studentId, courseId);
            else
                _logger.LogInformation("Enrollment found for StudentID {StudentId} and CourseID {CourseId}", studentId, courseId);

            return enrollment;
        }

        public async Task AddAsync(Enrollment enrollment)
        {
            try
            {
                _logger.LogInformation("Adding new enrollment: StudentID {StudentId}, CourseID {CourseId}", enrollment.StudentId, enrollment.CourseId);
                await _db.Enrollments.AddAsync(enrollment);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Enrollment added successfully: StudentID {StudentId}, CourseID {CourseId}", enrollment.StudentId, enrollment.CourseId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding enrollment (StudentID {StudentId}, CourseID {CourseId}): {Error}", enrollment.StudentId, enrollment.CourseId, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            try
            {
                _logger.LogInformation("Updating enrollment: StudentID {StudentId}, CourseID {CourseId}", enrollment.StudentId, enrollment.CourseId);
                _db.Enrollments.Update(enrollment);
                var affected = await _db.SaveChangesAsync();

                if (affected > 0)
                    _logger.LogInformation("Enrollment updated successfully: StudentID {StudentId}, CourseID {CourseId}", enrollment.StudentId, enrollment.CourseId);
                else
                    _logger.LogWarning("No records updated for enrollment: StudentID {StudentId}, CourseID {CourseId}", enrollment.StudentId, enrollment.CourseId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating enrollment (StudentID {StudentId}, CourseID {CourseId}): {Error}", enrollment.StudentId, enrollment.CourseId, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(int studentId, int courseId)
        {
            try
            {
                _logger.LogWarning("Attempting to delete enrollment: StudentID {StudentId}, CourseID {CourseId}", studentId, courseId);
                var enrollment = await _db.Enrollments
                    .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

                if (enrollment == null)
                {
                    _logger.LogWarning("Enrollment not found for deletion: StudentID {StudentId}, CourseID {CourseId}", studentId, courseId);
                    return;
                }

                _db.Enrollments.Remove(enrollment);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Enrollment deleted successfully: StudentID {StudentId}, CourseID {CourseId}", studentId, courseId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting enrollment (StudentID {StudentId}, CourseID {CourseId}): {Error}", studentId, courseId, ex.Message);
                throw;
            }
        }
    }
}
