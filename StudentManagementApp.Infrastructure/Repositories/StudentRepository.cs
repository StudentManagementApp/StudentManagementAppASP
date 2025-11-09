using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Infrastructure.Data;
using StudentManagementApp.Application.Common.Interfaces;

namespace StudentManagementApp.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        private readonly IAppLogger<StudentRepository> _logger;

        public StudentRepository(AppDbContext context, IAppLogger<StudentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all students from database...");

            var students = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ToListAsync();

            _logger.LogInformation("Fetched {Count} students from database", students.Count);
            return students;
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching student with ID {Id}", id);

            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                _logger.LogWarning("Student with ID {Id} not found", id);
            else
                _logger.LogInformation("Successfully retrieved student with ID {Id}", id);

            return student;
        }

        public async Task AddAsync(Student student)
        {
            try
            {
                _logger.LogInformation("Adding new student {Name} ({Email}) to database", student.FirstName + " " + student.LastName, student.Email);
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Student {Name} added successfully with ID {Id}", student.FirstName + " " + student.LastName, student.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding student {Name}: {Error}", student.FirstName + " " + student.LastName, ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Student student)
        {
            try
            {
                _logger.LogInformation("Updating student with ID {Id}", student.Id);

                if (!await _context.Students.AnyAsync(s => s.Id == student.Id))
                {
                    _logger.LogWarning("Cannot update. Student with ID {Id} does not exist.", student.Id);
                    return false;
                }

                _context.Students.Update(student);
                var affected = await _context.SaveChangesAsync();

                if (affected > 0)
                    _logger.LogInformation("Student with ID {Id} updated successfully", student.Id);
                else
                    _logger.LogWarning("No changes detected while updating student with ID {Id}", student.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating student with ID {Id}: {Error}", student.Id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogWarning("Attempting to delete student with ID {Id}", id);

                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    _logger.LogWarning("Student with ID {Id} not found for deletion", id);
                    return false;
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Student with ID {Id} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting student with ID {Id}: {Error}", id, ex.Message);
                throw;
            }
        }
    }
}
