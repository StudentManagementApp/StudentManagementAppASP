using StudentManagementApp.Domain.Entities;

namespace StudentManagementApp.Domain.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment> GetAsync(int studentId, int courseId);
        Task AddAsync(Enrollment enrollment);
        Task UpdateAsync(Enrollment enrollment);
        Task DeleteAsync(int studentId, int courseId);
    }
}
