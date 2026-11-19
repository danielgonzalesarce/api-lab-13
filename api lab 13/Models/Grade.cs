using System.ComponentModel.DataAnnotations;

namespace api_lab_13.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        [MaxLength(45)]
        public string Name { get; set; }

        [MaxLength(45)]
        public string Description { get; set; }

        public bool isActive { get; set; }

        public List<Student> Students { get; set; }
    }
}