using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Raceday.Data;
using Raceday.Models;
using System.Security.Claims;
using Route = Raceday.Models.Route;

namespace Raceday.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public EventsController(ApplicationDBContext context) => _context = context;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _context.Events.ToListAsync());
        [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => Ok(await _context.Events.Include(e => e.Organiser).FirstOrDefaultAsync(e => e.Event_ID == id));

        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<IActionResult> Create(Events ev)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var org = await _context.Organisers.FirstAsync(o => o.User_ID == userId);
            ev.Organiser_ID = org.Organiser_ID;
            _context.Events.Add(ev); await _context.SaveChangesAsync(); return Ok(ev);
        }

        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Events ev)
        {
            var ex = await _context.Events.FindAsync(id); if (ex == null) return NotFound();
            ex.Event_Name = ev.Event_Name; ex.Event_Description = ev.Event_Description; ex.Event_Venue = ev.Event_Venue; ex.Event_StartTime = ev.Event_StartTime;
            await _context.SaveChangesAsync(); return Ok(ex);
        }

        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Events.FindAsync(id); if (ev == null) return NotFound();
            _context.Events.Remove(ev); await _context.SaveChangesAsync(); return Ok();
        }

        [HttpGet("{eventId}/categories")] public async Task<IActionResult> GetCategories(int eventId) => Ok(await _context.Categories.Where(c => c.Event_ID == eventId).ToListAsync());
        [Authorize(Roles = "Organiser")]
        [HttpPost("{eventId}/categories")] public async Task<IActionResult> CreateCategory(int eventId, Category cat) { cat.Event_ID = eventId; _context.Categories.Add(cat); await _context.SaveChangesAsync(); return Ok(cat); }

        [HttpGet("{eventId}/routes")] public async Task<IActionResult> GetRoutes(int eventId) => Ok(await _context.Routes.Where(r => r.Event_ID == eventId).ToListAsync());
        [Authorize(Roles = "Organiser")]
        [HttpPost("{eventId}/routes")] public async Task<IActionResult> CreateRoute(int eventId, Route route) { route.Event_ID = eventId; _context.Routes.Add(route); await _context.SaveChangesAsync(); return Ok(route); }

        [HttpGet("{eventId}/results")]
        public async Task<IActionResult> GetResults(int eventId)
        {
            var results = await _context.Results.Include(r => r.Enrolment).ThenInclude(e => e.Category).Where(r => r.Enrolment!.Category!.Event_ID == eventId).ToListAsync();
            return Ok(results);
        }

        [HttpGet("{eventId}/enrolments")]
        public async Task<IActionResult> GetEnrolments(int eventId)
        {
            var enrols = await _context.Enrolments.Include(e => e.Participant).Include(e => e.Category).Where(e => e.Category!.Event_ID == eventId).ToListAsync();
            return Ok(enrols);
        }
    }
}