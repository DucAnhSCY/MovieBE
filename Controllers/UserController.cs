using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingTIcket.Models2;

namespace MovieBookingTIcket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DBContextTest _context;

        public UserController(DBContextTest context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WebUser>>> GetUsers()
        {
            var users = await _context.WebUsers.ToListAsync();
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WebUser>> GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be null or empty");
            }

            var user = await _context.WebUsers
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Show)
                .ThenInclude(s => s.Movie)
                .FirstOrDefaultAsync(m => m.WebUserId == id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }

            return Ok(user);
        }

        private bool UserExists(string id)
        {
            return _context.WebUsers.Any(e => e.WebUserId == id);
        }
    }
}
