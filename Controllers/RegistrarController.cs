using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TmsApi.Data;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/registrar")]
public class RegistrarController(TmsDbContext context) : ControllerBase
{
    // =========================================================================
    // SESSION 1 QUERIES
    // =========================================================================

    // Q1: Active students with GPA >= 3.0
    [HttpGet("active-high-gpa-count")]
    public async Task<IActionResult> GetActiveHighGpaCount()
    {
        var count = await context.Students
            .Where(s => s.IsActive && s.GPA >= 3.0m)
            .CountAsync();

        return Ok(new { ActiveHighGpaCount = count });
    }

    // Q2: Courses with the most enrollments, sorted descending
    [HttpGet("courses-by-enrollment")]
    public async Task<IActionResult> GetCoursesByEnrollment()
    {
        var list = await context.Courses
            .Select(c => new
            {
                c.Title,
                EnrollmentCount = c.Enrollments.Count
            })
            .OrderByDescending(x => x.EnrollmentCount)
            .ToListAsync();

        return Ok(list);
    }

    // Q3: Average GPA per course
    [HttpGet("average-gpa-per-course")]
    public async Task<IActionResult> GetAverageGpaPerCourse()
    {
        var list = await context.Enrollments
            .GroupBy(e => e.Course.Title)
            .Select(g => new
            {
                Course = g.Key,
                AverageGPA = g.Average(e => e.Student.GPA)
            })
            .ToListAsync();

        return Ok(list);
    }

    // Q4: Unenrolled students using a Subquery
    [HttpGet("unenrolled-students")]
    public async Task<IActionResult> GetUnenrolledStudents()
    {
        var list = await context.Students
            .Where(s => !s.Enrollments.Any())
            .Select(s => s.Name)
            .ToListAsync();

        return Ok(list);
    }

    // =========================================================================
    // SESSION 2 QUERIES (EXERCISE 3)
    // =========================================================================

    // TODO 1: Paginated Student Roster
    [HttpGet("students-paged")]
    public async Task<IActionResult> GetStudentsPaged(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        var students = await context.Students
            .OrderBy(s => s.Name) 
            .Skip((page - 1) * pageSize) 
            .Take(pageSize) 
            .ToListAsync(cancellationToken);

        return Ok(students);
    }

    // TODO 2: Top 5 Courses by Enrollment Count
    [HttpGet("top-courses")]
    public async Task<IActionResult> GetTopCourses(CancellationToken cancellationToken = default)
    {
        var topCourses = await context.Courses
            .Select(c => new
            {
                CourseTitle = c.Title,
                EnrollmentCount = c.Enrollments.Count
            })
            .OrderByDescending(x => x.EnrollmentCount) 
            .Take(5) 
            .ToListAsync(cancellationToken);

        return Ok(topCourses);
    }
}
