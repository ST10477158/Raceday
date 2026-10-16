using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday.Data;
using Raceday.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Raceday.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public CategoriesController(ApplicationDBContext context) => _context = context;

        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category cat)
        {
            var ex = await _context.Categories.FindAsync(id); if (ex == null) return NotFound();
            ex.Category_Name = cat.Category_Name; ex.Category_EntryAmount = cat.Category_EntryAmount; ex.Category_QualifyAge = cat.Category_QualifyAge;
            await _context.SaveChangesAsync(); return Ok(ex);
        }

        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.Categories.FindAsync(id); if (c == null) return NotFound();
            if (await _context.Enrolments.AnyAsync(e => e.Category_ID == id)) return BadRequest("Cannot delete category with enrolments");
            _context.Categories.Remove(c); await _context.SaveChangesAsync(); return Ok();
        }

        [Route("/api/events/{eventId}/categories/{categoryId}/enrolments")]
        [Authorize(Roles = "Participant")]
        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var part = await _context.Participants.FirstAsync(p => p.User_ID == userId);
            var enrol = new Enrolment { Participant_ID = part.Participant_ID, Category_ID = categoryId, Enrolment_Date = DateTime.UtcNow, Race_Number = new Random().Next(100, 999) };
            _context.Enrolments.Add(enrol); await _context.SaveChangesAsync(); return Ok(enrol);
        }
    }
}