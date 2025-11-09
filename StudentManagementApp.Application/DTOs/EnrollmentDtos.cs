namespace StudentManagementApp.Application.DTOs
{
    public class EnrollmentDto
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public decimal? Grade { get; set; }
    }

    public class CreateEnrollmentDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal? Grade { get; set; }
    }

    public class UpdateEnrollmentDto
    {
        public decimal? Grade { get; set; }
    }
}
