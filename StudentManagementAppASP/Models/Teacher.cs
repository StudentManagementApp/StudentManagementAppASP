using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace StudentManagementAppASP.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // One-to-Many relationship
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
