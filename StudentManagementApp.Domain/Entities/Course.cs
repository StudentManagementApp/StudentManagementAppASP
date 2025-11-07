using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementApp.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        // One-to-Many: Teacher -> Courses
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        // Many-to-Many: Students <-> Courses
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
