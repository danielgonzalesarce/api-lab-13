namespace api_lab_13.DTO.Response
{
    public class StudentGradeResponseV1
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public GradeResponseV1 Grade { get; set; }
    }
}