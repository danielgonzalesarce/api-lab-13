using api_lab_13.DTO.Request;
using api_lab_13.DTO.Response;
using api_lab_13.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_lab_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Courses (solo activos)
        [HttpGet]
        public IActionResult GetCourses()
        {
            var courses = _context.Courses
                .Where(c => c.isActive)
                .Select(c => new CourseResponseV1
                {
                    CourseId = c.CourseId,
                    Name = c.Name,
                    Credit = c.Credit,
                    isActive = c.isActive
                })
                .ToList();

            return Ok(courses);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public IActionResult GetCourse(int id)
        {
            var course = _context.Courses
                .Where(c => c.CourseId == id && c.isActive)
                .Select(c => new CourseResponseV1
                {
                    CourseId = c.CourseId,
                    Name = c.Name,
                    Credit = c.Credit,
                    isActive = c.isActive
                })
                .FirstOrDefault();

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // POST: api/Courses
        [HttpPost]
        public IActionResult PostCourse(CourseRequestV1 request)
        {
            var course = new Course
            {
                Name = request.Name,
                Credit = request.Credit,
                isActive = true
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            var response = new CourseResponseV1
            {
                CourseId = course.CourseId,
                Name = course.Name,
                Credit = course.Credit,
                isActive = course.isActive
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, response);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public IActionResult PutCourse(int id, CourseRequestV1 request)
        {
            var course = _context.Courses
                .FirstOrDefault(c => c.CourseId == id && c.isActive);

            if (course == null)
            {
                return NotFound();
            }

            course.Name = request.Name;
            course.Credit = request.Credit;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Courses/5 (eliminación lógica)
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses
                .FirstOrDefault(c => c.CourseId == id && c.isActive);

            if (course == null)
            {
                return NotFound();
            }

            course.isActive = false;

            _context.SaveChanges();

            return NoContent();
        }
    }
}