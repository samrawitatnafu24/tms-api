using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TmsApi.Entities;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly DbContext _context;

    public StudentsController(DbContext context)
    {
        _context = context;
    }

    // Exercise 7 Part A: N+1 Trap Demonstration
    [HttpGet("n-plus-one-trap")]
    public async Task<IActionResult> GetStudentsNPlusOne(CancellationToken cancellationToken)
    {
        var students = await _context.Set<Student>().AsNoTracking().ToListAsync(cancellationToken);
        var resultList = new List<object>();

        foreach (var s in students)
        {
            var count = await _context.Set<Enrollment>()
                .AsNoTracking()
                .CountAsync(e => e.StudentId == s.Id, cancellationToken);
                
            resultList.Add(new { StudentName = s.Name, EnrollmentCount = count });
        }
        return Ok(resultList);
    }

    // Exercise 7 Part B: N+1 Fixed with Projection
    [HttpGet("n-plus-one-fixed")]
    public async Task<IActionResult> GetStudentsFixed(CancellationToken cancellationToken)
    {
        var report = await _context.Set<Student>()
            .AsNoTracking()
            .Select(s => new
            {
                StudentName = s.Name,
                EnrollmentCount = s.Enrollments.Count
            })
            .ToListAsync(cancellationToken);

        return Ok(report);
    }

    // Exercise 9: Normal Query (Soft-deleted students disappear automatically)
    [HttpGet("active-only")]
    public async Task<IActionResult> GetActiveStudents()
    {
        var activeStudents = await _context.Set<Student>().AsNoTracking().ToListAsync();
        return Ok(activeStudents);
    }

    // Exercise 9: Admin Scenario (Bypass filter using IgnoreQueryFilters)
    [HttpGet("admin-all-including-deleted")]
    public async Task<IActionResult> GetAllStudentsForAdmin()
    {
        var allStudents = await _context.Set<Student>()
            .IgnoreQueryFilters()
            .AsNoTracking()
            .ToListAsync();
        return Ok(allStudents);
    }
}
