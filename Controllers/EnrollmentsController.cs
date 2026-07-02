using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TmsApi.Entities;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly DbContext _context; 

    // Directly inject the DbContext to bypass the broken service layer contract
    public EnrollmentsController(DbContext context)
    {
        _context = context;
    }

    // GET: api/enrollments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Enrollment>>> GetAll()
    {
        var records = await _context.Set<Enrollment>().AsNoTracking().ToListAsync();
        return Ok(records);
    }

    // GET: api/enrollments/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Enrollment>> GetById(int id)
    {
        var record = await _context.Set<Enrollment>().FindAsync(id);
        if (record == null)
        {
            return NotFound();
        }
        return Ok(record);
    }

    // POST: api/enrollments
    [HttpPost]
    public async Task<ActionResult<Enrollment>> Enroll([FromBody] Enrollment record)
    {
        _context.Set<Enrollment>().Add(record);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
    }

    // DELETE: api/enrollments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _context.Set<Enrollment>().FindAsync(id);
        if (record == null)
        {
            return NotFound();
        }

        _context.Set<Enrollment>().Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
    } 

    /// <summary>
    /// Exercise 9: High-performance bulk archive using ExecuteUpdateAsync
    /// </summary>
    [HttpPost("bulk-archive-old")]
    public async Task<IActionResult> BulkArchiveOldEnrollments([FromQuery] int daysOld)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);

        // Modifies thousands of target rows directly in PostgreSQL using a single high-performance query statement
        int rowsAffected = await _context.Set<Enrollment>()
            .Where(e => e.EnrolledAt < cutoffDate && !e.IsArchived)
            .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsArchived, true));

        return Ok(new { Message = "Bulk archiving complete.", RowsArchived = rowsAffected });
    }
}
