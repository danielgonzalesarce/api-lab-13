using System.ComponentModel.DataAnnotations;

namespace api_lab_13.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [MaxLength(45)]
        public string Name { get; set; }

        public int Credit { get; set; }

        public bool isActive { get; set; }

        public List<Enrollment> Enrollments { get; set; }
    }
}