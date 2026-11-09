using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Filters;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;
namespace RaceDay.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiService _api;
        public ParticipantController(ApiService api) => _api = api;
        public async Task<IActionResult> Home(string? search, string? venue)
        {
            var events = await _api.GetAllEventsAsync();
            events = events.Where(e => e.Event_StartTime >= DateTime.Now).ToList();
            if (!string.IsNullOrEmpty(search)) events = events.Where(e => e.Event_Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrEmpty(venue)) events = events.Where(e => e.Event_Venue.Contains(venue, StringComparison.OrdinalIgnoreCase)).ToList();
            return View(events);
        }
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _api.GetEventAsync(id); if (ev == null) return NotFound();
            ViewBag.Categories = await _api.GetCategoriesAsync(id);
            ViewBag.Routes = await _api.GetRoutesAsync(id);
            return View(ev);
        }
        [SessionAuthorize(Roles = "Participant")]
        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            await _api.EnrolAsync(eventId, categoryId);
            return RedirectToAction("MyEnrolments");
        }
        [SessionAuthorize(Roles = "Participant")]
        public async Task<IActionResult> MyEnrolments()
        {
            int pid = int.Parse(HttpContext.Session.GetString("Participant_ID")!);
            var my = await _api.GetMyEnrolmentsAsync(pid);
            var events = await _api.GetAllEventsAsync();
            foreach (var e in my)
            {
                var cats = new List<CategoryViewModel>();
                foreach (var ev in events) cats.AddRange(await _api.GetCategoriesAsync(ev.Event_ID));
                var cat = cats.FirstOrDefault(c => c.Category_ID == e.Category_ID);
                if (cat != null) { e.Category_Name = cat.Category_Name; e.Category_EntryAmount = cat.Category_EntryAmount; e.Event_ID = cat.Event_ID; e.Event_Name = events.FirstOrDefault(x => x.Event_ID == cat.Event_ID)?.Event_Name ?? ""; }
            }
            return View(my);
        }
        [SessionAuthorize(Roles = "Participant")]
        public async Task<IActionResult> MyResults()
        {
            int pid = int.Parse(HttpContext.Session.GetString("Participant_ID")!);
            var myEnrols = await _api.GetMyEnrolmentsAsync(pid);
            var results = new List<ResultViewModel>();
            var events = await _api.GetAllEventsAsync();
            foreach (var ev in events) results.AddRange(await _api.GetResultsForEventAsync(ev.Event_ID));
            return View(results.Where(r => myEnrols.Any(e => e.Enrolment_ID == r.Enrolment_ID)).ToList());
        }
        [SessionAuthorize(Roles = "Participant")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            int uid = int.Parse(HttpContext.Session.GetString("User_ID")!);
            return View(await _api.GetProfileAsync(uid));
        }
        [SessionAuthorize(Roles = "Participant")]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel m)
        {
            if (await _api.UpdateProfileAsync(m)) return RedirectToAction("Profile");
            return View(m);
        }
    }
}