using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
namespace RaceDay.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _api;
        public HomeController(ApiService api) => _api = api;
        public async Task<IActionResult> Index()
        {
            var events = (await _api.GetAllEventsAsync()).Where(e => e.Event_StartTime >= DateTime.Now).OrderBy(e => e.Event_StartTime).Take(6).ToList();
            return View(events);
        }
    }
}