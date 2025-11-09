namespace StudentManagementApp.Application.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
    }

    public class CreateCourseDto
    {
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int TeacherId { get; set; }
    }

    public class UpdateCourseDto
    {
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int TeacherId { get; set; }
    }
}
