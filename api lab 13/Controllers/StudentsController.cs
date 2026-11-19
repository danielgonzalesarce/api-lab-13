using api_lab_13.DTO.Request;
using api_lab_13.DTO.Response;
using api_lab_13.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_lab_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Students
        [HttpPost]
        public IActionResult PostStudent(StudentRequestV1 request)
        {
            var gradeExists = _context.Grades
                .Any(g => g.GradeId == request.GradeId && g.isActive);

            if (!gradeExists)
            {
                return BadRequest("El grado no existe o está inactivo.");
            }

            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Email = request.Email,
                GradeId = request.GradeId
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            var response = new StudentResponseV1
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Phone = student.Phone,
                Email = student.Email,
                GradeId = student.GradeId
            };

            return Ok(response);
        }

        // Punto 8
        // GET: api/Students/search?firstName=ana&lastName=torres&email=ana
        // Busca por nombre, apellido y email, ordenado descendente por apellido
        [HttpGet("search")]
        public IActionResult SearchStudents(string firstName, string lastName, string email)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(s => s.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(s => s.LastName.Contains(lastName));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(s => s.Email.Contains(email));
            }

            var students = query
                .OrderByDescending(s => s.LastName)
                .Select(s => new StudentResponseV1
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Phone = s.Phone,
                    Email = s.Email,
                    GradeId = s.GradeId
                })
                .ToList();

            return Ok(students);
        }

        // Punto 9
        // GET: api/Students/search-by-grade?firstName=ana&gradeName=primer
        // Busca por nombre y grado, devuelve datos del estudiante y de su grado,
        // ordenado descendente por nombre del grado
        [HttpGet("search-by-grade")]
        public IActionResult SearchStudentsByGrade(string firstName, string gradeName)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(s => s.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrWhiteSpace(gradeName))
            {
                query = query.Where(s => s.Grade.Name.Contains(gradeName));
            }

            var students = query
                .OrderByDescending(s => s.Grade.Name)
                .Select(s => new StudentGradeResponseV1
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Phone = s.Phone,
                    Email = s.Email,
                    Grade = new GradeResponseV1
                    {
                        GradeId = s.Grade.GradeId,
                        Name = s.Grade.Name,
                        Description = s.Grade.Description,
                        isActive = s.Grade.isActive
                    }
                })
                .ToList();

            return Ok(students);
        }
    }
}