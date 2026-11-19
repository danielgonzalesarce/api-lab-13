using System.ComponentModel.DataAnnotations;

namespace api_lab_13.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [MaxLength(45)]
        public string FirstName { get; set; }

        [MaxLength(45)]
        public string LastName { get; set; }

        [MaxLength(45)]
        public string Phone { get; set; }

        [MaxLength(45)]
        public string Email { get; set; }

        public int GradeId { get; set; }
        public Grade Grade { get; set; }

        public List<Enrollment> Enrollments { get; set; }
    }
}