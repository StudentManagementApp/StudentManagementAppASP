using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Domain.Entities;

namespace StudentManagementApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Key for Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.StudentId, e.CourseId });

            // One-to-Many: Teacher -> Courses
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId);

            // Many-to-Many: Student <-> Course (via Enrollment)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);


            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, FirstName = "Ali ", LastName="Reazai" , BirthDate = new DateTime(2000, 5, 20), Email = "ali@example.com" ,  Phone = "09120006574" },
                new Student { Id = 2, FirstName = "Sara", LastName="Rahimi" , BirthDate = new DateTime(2001, 3, 14), Email = "sara@example.com", Phone = "09121105270" },
                new Student { Id = 3, FirstName = "Hamid", LastName="Zahrai" , BirthDate = new DateTime(2007, 7, 19), Email = "Hamid@example.com", Phone = "09120942002" }
            );
        }
    }
}
