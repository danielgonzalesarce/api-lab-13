namespace api_lab_13.DTO.Response
{
    public class EnrollmentDetailResponseV1
    {
        public int EnrollmentId { get; set; }
        public DateTime Date { get; set; }
        public StudentResponseV1 Student { get; set; }
        public CourseResponseV1 Course { get; set; }
    }
}