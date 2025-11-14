using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementApp.Domain.Interfaces;
using StudentManagementApp.Infrastructure.Data;
using StudentManagementApp.Infrastructure.Repositories;
using StudentManagementApp.Infrastructure.UnitOfWork;
namespace StudentManagementApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("StudentManagementApp.Infrastructure")
                )
            );

            // Register Repositories
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

            // Register UnitOfWork  (FULL NAME to avoid namespace conflict)
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}
