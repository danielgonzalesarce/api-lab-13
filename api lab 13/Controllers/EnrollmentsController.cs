using api_lab_13.DTO.Request;
using api_lab_13.DTO.Response;
using api_lab_13.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_lab_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Enrollments
        [HttpPost]
        public IActionResult PostEnrollment(EnrollmentRequestV1 request)
        {
            var studentExists = _context.Students
                .Any(s => s.StudentId == request.StudentId);

            if (!studentExists)
            {
                return BadRequest("El estudiante no existe.");
            }

            var courseExists = _context.Courses
                .Any(c => c.CourseId == request.CourseId && c.isActive);

            if (!courseExists)
            {
                return BadRequest("El curso no existe o está inactivo.");
            }

            var enrollment = new Enrollment
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                Date = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            var response = new EnrollmentResponseV1
            {
                EnrollmentId = enrollment.EnrollmentId,
                Date = enrollment.Date,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId
            };

            return Ok(response);
        }

        // Punto 10
        // GET: api/Enrollments/by-course?courseName=program
        // Matriculados por nombre de curso, ordenados por curso y luego por apellido
        [HttpGet("by-course")]
        public IActionResult GetEnrollmentsByCourse(string courseName)
        {
            var query = _context.Enrollments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(courseName))
            {
                query = query.Where(e => e.Course.Name.Contains(courseName));
            }

            var enrollments = query
                .OrderBy(e => e.Course.Name)
                .ThenBy(e => e.Student.LastName)
                .Select(e => new EnrollmentDetailResponseV1
                {
                    EnrollmentId = e.EnrollmentId,
                    Date = e.Date,
                    Student = new StudentResponseV1
                    {
                        StudentId = e.Student.StudentId,
                        FirstName = e.Student.FirstName,
                        LastName = e.Student.LastName,
                        Phone = e.Student.Phone,
                        Email = e.Student.Email,
                        GradeId = e.Student.GradeId
                    },
                    Course = new CourseResponseV1
                    {
                        CourseId = e.Course.CourseId,
                        Name = e.Course.Name,
                        Credit = e.Course.Credit,
                        isActive = e.Course.isActive
                    }
                })
                .ToList();

            return Ok(enrollments);
        }

        // Punto 11
        // GET: api/Enrollments/by-grade?gradeName=primer
        // Matriculados por grado, ordenados por curso y luego por apellido
        [HttpGet("by-grade")]
        public IActionResult GetEnrollmentsByGrade(string gradeName)
        {
            var query = _context.Enrollments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(gradeName))
            {
                query = query.Where(e => e.Student.Grade.Name.Contains(gradeName));
            }

            var enrollments = query
                .OrderBy(e => e.Course.Name)
                .ThenBy(e => e.Student.LastName)
                .Select(e => new EnrollmentDetailResponseV1
                {
                    EnrollmentId = e.EnrollmentId,
                    Date = e.Date,
                    Student = new StudentResponseV1
                    {
                        StudentId = e.Student.StudentId,
                        FirstName = e.Student.FirstName,
                        LastName = e.Student.LastName,
                        Phone = e.Student.Phone,
                        Email = e.Student.Email,
                        GradeId = e.Student.GradeId
                    },
                    Course = new CourseResponseV1
                    {
                        CourseId = e.Course.CourseId,
                        Name = e.Course.Name,
                        Credit = e.Course.Credit,
                        isActive = e.Course.isActive
                    }
                })
                .ToList();

            return Ok(enrollments);
        }
    }
}