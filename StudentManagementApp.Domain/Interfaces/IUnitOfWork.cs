using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagementApp.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }
        ICourseRepository Courses { get; }
        IEnrollmentRepository Enrollments { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
