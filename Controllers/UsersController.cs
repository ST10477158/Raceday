using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday.Data;
using Raceday.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Raceday.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UsersController(ApplicationDBContext context) => _context = context;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (id != userId) return Forbid();
            return Ok(await _context.Users.FindAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, User updated)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (id != userId) return Forbid();
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            user.User_Email = updated.User_Email;
            if (!string.IsNullOrEmpty(updated.User_Password))
                user.User_Password = BCrypt.Net.BCrypt.HashPassword(updated.User_Password);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}