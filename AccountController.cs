using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;
namespace RaceDay.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _api;
        public AccountController(ApiService api) => _api = api;
        [HttpGet] public IActionResult Login() => View();
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel m)
        {
            if (!ModelState.IsValid) return View(m);
            var r = await _api.LoginAsync(m);
            if (!r.Success) { ModelState.AddModelError("", "Invalid email or password"); return View(m); }
            HttpContext.Session.SetString("User_ID", r.User_ID.ToString());
            HttpContext.Session.SetString("User_Role", r.User_Role);
            HttpContext.Session.SetString("User_Email", r.User_Email);
            HttpContext.Session.SetString("Participant_ID", r.Participant_ID.ToString());
            HttpContext.Session.SetString("Organiser_ID", r.Organiser_ID.ToString());
            HttpContext.Session.SetString("DisplayName", r.DisplayName);
            return r.User_Role == "Organiser" ? RedirectToAction("Dashboard", "Organiser") : RedirectToAction("Home", "Participant");
        }
        [HttpGet] public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel m)
        {
            if (!ModelState.IsValid) return View(m);
            if (await _api.RegisterAsync(m)) return RedirectToAction("Login");
            ModelState.AddModelError("", "Email already exists"); return View(m);
        }
        public IActionResult Logout() { HttpContext.Session.Clear(); return RedirectToAction("Index", "Home"); }
    }
}