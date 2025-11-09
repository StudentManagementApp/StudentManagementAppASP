using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Domain.Entities;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Infrastructure.Data;
using StudentManagementApp.Application.Common.Interfaces;

namespace StudentManagementApp.Infrastructure.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _context;
        private readonly IAppLogger<TeacherRepository> _logger;

        public TeacherRepository(AppDbContext context, IAppLogger<TeacherRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all teachers from database...");
            var teachers = await _context.Teachers.ToListAsync();
            _logger.LogInformation("Fetched {Count} teachers from database", teachers.Count);
            return teachers;
        }

        public async Task<Teacher?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching teacher with ID {Id}", id);
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
                _logger.LogWarning("Teacher with ID {Id} not found", id);
            else
                _logger.LogInformation("Successfully retrieved teacher with ID {Id}", id);

            return teacher;
        }

        public async Task AddAsync(Teacher teacher)
        {
            try
            {
                _logger.LogInformation("Adding new teacher '{Name}' ({Email}) to database", teacher.Name, teacher.Email);
                await _context.Teachers.AddAsync(teacher);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Teacher '{Name}' added successfully with ID {Id}", teacher.Name, teacher.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding teacher '{Name}': {Error}", teacher.Name, ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Teacher teacher)
        {
            try
            {
                _logger.LogInformation("Updating teacher with ID {Id}", teacher.Id);

                if (!await _context.Teachers.AnyAsync(t => t.Id == teacher.Id))
                {
                    _logger.LogWarning("Cannot update. Teacher with ID {Id} not found", teacher.Id);
                    return false;
                }

                _context.Teachers.Update(teacher);
                var affected = await _context.SaveChangesAsync();

                if (affected > 0)
                    _logger.LogInformation("Teacher with ID {Id} updated successfully", teacher.Id);
                else
                    _logger.LogWarning("No changes detected while updating teacher with ID {Id}", teacher.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating teacher with ID {Id}: {Error}", teacher.Id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogWarning("Attempting to delete teacher with ID {Id}", id);
                var teacher = await _context.Teachers.FindAsync(id);

                if (teacher == null)
                {
                    _logger.LogWarning("Teacher with ID {Id} not found for deletion", id);  
                    return false;
                }

                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Teacher with ID {Id} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting teacher with ID {Id}: {Error}", id, ex.Message);
                throw;
            }
        }
    }
}
