using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace StudentManagementApp.Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // One-to-Many relationship
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
