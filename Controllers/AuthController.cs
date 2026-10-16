using Microsoft.AspNetCore.Mvc;
using Raceday.Data;
using Raceday.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Raceday.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public AuthController(ApplicationDBContext context) => _context = context;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.User_Email == user.User_Email))
                return BadRequest("Email exists");
            user.User_Password = BCrypt.Net.BCrypt.HashPassword(user.User_Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            if (user.User_Role == "Participant")
                _context.Participants.Add(new Participant { User_ID = user.User_ID, Participant_Name = user.User_Email.Split('@')[0], Participant_Surname = "User" });
            else
                _context.Organisers.Add(new Organiser { User_ID = user.User_ID, Organisation_Name = user.User_Email, Organisation_Phone = "0000000" });
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_Email == login.User_Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.User_Password, user.User_Password))
                return Unauthorized("Invalid");
            var claims = new List<Claim> {
 new Claim(ClaimTypes.NameIdentifier, user.User_ID.ToString()),
 new Claim(ClaimTypes.Name, user.User_Email),
 new Claim(ClaimTypes.Role, user.User_Role)
 };
            await HttpContext.SignInAsync("RacedayAuth", new ClaimsPrincipal(new ClaimsIdentity(claims, "RacedayAuth")));
            return Ok(new { user.User_ID, user.User_Email, user.User_Role });
        }
    }
}