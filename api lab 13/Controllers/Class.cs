using api_lab_13.DTO.Request;
using api_lab_13.DTO.Response;
using api_lab_13.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_lab_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Grades (solo activos)
        [HttpGet]
        public IActionResult GetGrades()
        {
            var grades = _context.Grades
                .Where(g => g.isActive)
                .Select(g => new GradeResponseV1
                {
                    GradeId = g.GradeId,
                    Name = g.Name,
                    Description = g.Description,
                    isActive = g.isActive
                })
                .ToList();

            return Ok(grades);
        }

        // GET: api/Grades/5
        [HttpGet("{id}")]
        public IActionResult GetGrade(int id)
        {
            var grade = _context.Grades
                .Where(g => g.GradeId == id && g.isActive)
                .Select(g => new GradeResponseV1
                {
                    GradeId = g.GradeId,
                    Name = g.Name,
                    Description = g.Description,
                    isActive = g.isActive
                })
                .FirstOrDefault();

            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);
        }

        // POST: api/Grades
        [HttpPost]
        public IActionResult PostGrade(GradeRequestV1 request)
        {
            var grade = new Grade
            {
                Name = request.Name,
                Description = request.Description,
                isActive = true
            };

            _context.Grades.Add(grade);
            _context.SaveChanges();

            var response = new GradeResponseV1
            {
                GradeId = grade.GradeId,
                Name = grade.Name,
                Description = grade.Description,
                isActive = grade.isActive
            };

            return CreatedAtAction(nameof(GetGrade), new { id = grade.GradeId }, response);
        }

        // PUT: api/Grades/5
        [HttpPut("{id}")]
        public IActionResult PutGrade(int id, GradeRequestV1 request)
        {
            var grade = _context.Grades
                .FirstOrDefault(g => g.GradeId == id && g.isActive);

            if (grade == null)
            {
                return NotFound();
            }

            grade.Name = request.Name;
            grade.Description = request.Description;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Grades/5 (eliminación lógica)
        [HttpDelete("{id}")]
        public IActionResult DeleteGrade(int id)
        {
            var grade = _context.Grades
                .FirstOrDefault(g => g.GradeId == id && g.isActive);

            if (grade == null)
            {
                return NotFound();
            }

            grade.isActive = false;

            _context.SaveChanges();

            return NoContent();
        }
    }
}