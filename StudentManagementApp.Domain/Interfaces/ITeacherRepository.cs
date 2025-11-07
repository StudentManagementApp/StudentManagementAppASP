using StudentManagementApp.Domain.Entities;

namespace StudentManagementApp.Domain.Interfaces
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher?> GetByIdAsync(int id);
        Task AddAsync(Teacher teacher);
        Task<bool> UpdateAsync(Teacher teacher);
        Task<bool> DeleteAsync(int id);
    }
}
