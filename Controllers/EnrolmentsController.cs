using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday.Data;
namespace Raceday.Controllers
{
    [Route("api/enrolments")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public EnrolmentsController(ApplicationDBContext context) => _context = context;
        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _context.Enrolments.Include(e => e.Category).ToListAsync());
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var e = await _context.Enrolments.FindAsync(id); if (e == null) return NotFound(); _context.Enrolments.Remove(e); await _context.SaveChangesAsync(); return Ok(); }
    }
}