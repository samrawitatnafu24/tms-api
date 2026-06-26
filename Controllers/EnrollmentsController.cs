using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TmsApi.Controlles
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        // Dependency Injection
        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // GET: api/enrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentRecord>>> GetAll()
        {
            var records = await _enrollmentService.GetAllAsync();
            return Ok(records);
        }

        // GET: api/enrollments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentRecord>> GetById(int id)
        {
            var record = await _enrollmentService.GetByIdAsync(id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        // POST: api/enrollments
        [HttpPost]
        public async Task<ActionResult<EnrollmentRecord>> Enroll([FromBody] EnrollmentRecord record)
        {
            var result = await _enrollmentService.EnrollAsync(record);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // DELETE: api/enrollments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _enrollmentService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
