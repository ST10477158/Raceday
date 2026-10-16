using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday.Data;
using Raceday.Models;
using Microsoft.AspNetCore.Authorization;
using Results = Raceday.Models.Results;
namespace Raceday.Controllers
{
    [Route("api/results")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public ResultsController(ApplicationDBContext context) => _context = context;

        [Authorize(Roles = "Organiser")]
        [HttpPost("{enrolmentId}")]
        public async Task<IActionResult> Create(int enrolmentId, Results res) { res.Enrolment_ID = enrolmentId; _context.Results.Add(res); await _context.SaveChangesAsync(); return Ok(res); }

        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Results res) { var ex = await _context.Results.FindAsync(id); if (ex == null) return NotFound(); ex.Finish_Time = res.Finish_Time; ex.Finish_Position = res.Finish_Position; ex.Average_Pace = res.Average_Pace; ex.Category_Position = res.Category_Position; await _context.SaveChangesAsync(); return Ok(ex); }

        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var r = await _context.Results.FindAsync(id); if (r == null) return NotFound(); _context.Results.Remove(r); await _context.SaveChangesAsync(); return Ok(); }
    }
}