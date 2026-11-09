using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Filters;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;
namespace RaceDay.Web.Controllers
{
    [SessionAuthorize(Roles = "Organiser")]
    public class OrganiserController : Controller
    {
        private readonly ApiService _api;
        public OrganiserController(ApiService api) => _api = api;
        public async Task<IActionResult> Dashboard()
        {
            int orgId = int.Parse(HttpContext.Session.GetString("Organiser_ID")!);
            var events = (await _api.GetAllEventsAsync()).Where(e => e.Organiser_ID == orgId).ToList();
            ViewBag.TotalEvents = events.Count;
            ViewBag.Upcoming = events.Where(e => e.Event_StartTime >= DateTime.Now).OrderBy(e => e.Event_StartTime).Take(5).ToList();
            return View(events);
        }
        public async Task<IActionResult> Events()
        {
            int orgId = int.Parse(HttpContext.Session.GetString("Organiser_ID")!);
            return View((await _api.GetAllEventsAsync()).Where(e => e.Organiser_ID == orgId).ToList());
        }
        [HttpGet] public IActionResult CreateEvent() => View(new EventViewModel());
        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventViewModel m)
        {
            if (!ModelState.IsValid) return View(m);
            if (await _api.CreateEventAsync(m)) return RedirectToAction("Events");
            ModelState.AddModelError("", "Could not create event"); return View(m);
        }
        [HttpGet] public async Task<IActionResult> EditEvent(int id) => View(await _api.GetEventAsync(id));
        [HttpPost]
        public async Task<IActionResult> EditEvent(EventViewModel m)
        {
            if (await _api.UpdateEventAsync(m)) return RedirectToAction("Events");
            return View(m);
        }
        public async Task<IActionResult> DeleteEvent(int id) { await _api.DeleteEventAsync(id); return RedirectToAction("Events"); }
        public async Task<IActionResult> Categories(int eventId) { ViewBag.Event_ID = eventId; return View(await _api.GetCategoriesAsync(eventId)); }
        [HttpPost] public async Task<IActionResult> AddCategory(CategoryViewModel m) { await _api.AddCategoryAsync(m); return RedirectToAction("Categories", new { eventId = m.Event_ID }); }
        public async Task<IActionResult> Routes(int eventId) { ViewBag.Event_ID = eventId; return View(await _api.GetRoutesAsync(eventId)); }
        [HttpPost] public async Task<IActionResult> AddRoute(RouteViewModel m) { await _api.AddRouteAsync(m); return RedirectToAction("Routes", new { eventId = m.Event_ID }); }
        public async Task<IActionResult> Enrolments(int eventId) { ViewBag.Event_ID = eventId; return View(await _api.GetEnrolmentsForEventAsync(eventId)); }
        public async Task<IActionResult> Results(int eventId) { ViewBag.Event_ID = eventId; return View(await _api.GetResultsForEventAsync(eventId)); }
        [HttpGet] public IActionResult AddResult(int enrolmentId, int eventId) { ViewBag.Event_ID = eventId; return View(new ResultViewModel { Enrolment_ID = enrolmentId, Event_ID = eventId }); }
        [HttpPost] public async Task<IActionResult> AddResult(ResultViewModel m) { await _api.AddResultAsync(m); return RedirectToAction("Enrolments", new { eventId = m.Event_ID }); }
    }
}