namespace StudentManagementApp.Application.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
    }

    public class CreateStudentDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
    }

    public class UpdateStudentDto : CreateStudentDto {}
}
