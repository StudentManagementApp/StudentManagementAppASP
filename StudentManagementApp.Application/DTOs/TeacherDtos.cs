namespace StudentManagementApp.Application.DTOs
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
    }

    public class CreateTeacherDto
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
    }

    public class UpdateTeacherDto : CreateTeacherDto {}
}
