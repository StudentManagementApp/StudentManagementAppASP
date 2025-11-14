using System.Threading;
using System.Threading.Tasks;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Infrastructure.Data;

namespace StudentManagementApp.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IStudentRepository Students { get; }
        public ITeacherRepository Teachers { get; }
        public ICourseRepository Courses { get; }
        public IEnrollmentRepository Enrollments { get; }

        public UnitOfWork(
            AppDbContext context,
            IStudentRepository students,
            ITeacherRepository teachers,
            ICourseRepository courses,
            IEnrollmentRepository enrollments)
        {
            _context = context;
            Students = students;
            Teachers = teachers;
            Courses = courses;
            Enrollments = enrollments;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _context.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
