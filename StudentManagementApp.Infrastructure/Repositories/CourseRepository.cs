using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Infrastructure.Data;
using StudentManagementApp.Application.Common.Interfaces;

namespace StudentManagementApp.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _db;
        private readonly IAppLogger<CourseRepository> _logger;

        public CourseRepository(AppDbContext db, IAppLogger<CourseRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all courses from database...");
            var courses = await _db.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Enrollments)
                .ToListAsync();

            _logger.LogInformation("Fetched {Count} courses from database", courses.Count);
            return courses;
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching course with ID {Id}", id);

            var course = await _db.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                _logger.LogWarning("Course with ID {Id} not found in database", id);
            else
                _logger.LogInformation("Successfully retrieved course with ID {Id}", id);

            return course;
        }

        public async Task AddAsync(Course course)
        {
            try
            {
                _logger.LogInformation("Adding new course '{Title}' to database", course.Title);
                await _db.Courses.AddAsync(course);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Course '{Title}' added successfully with ID {Id}", course.Title, course.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while adding course '{Title}': {Error}", course.Title, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(Course course)
        {
            try
            {
                _logger.LogInformation("Updating course with ID {Id}", course.Id);
                _db.Courses.Update(course);
                var affected = await _db.SaveChangesAsync();

                if (affected > 0)
                    _logger.LogInformation("Course with ID {Id} updated successfully", course.Id);
                else
                    _logger.LogWarning("No records updated for course with ID {Id}", course.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating course with ID {Id}: {Error}", course.Id, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                _logger.LogWarning("Attempting to delete course with ID {Id}", id);
                var course = await _db.Courses.FindAsync(id);

                if (course == null)
                {
                    _logger.LogWarning("Course with ID {Id} not found for deletion", id);
                    return;
                }

                _db.Courses.Remove(course);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Course with ID {Id} deleted successfully", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting course with ID {Id}: {Error}", id, ex.Message);
                throw;
            }
        }
    }
}
