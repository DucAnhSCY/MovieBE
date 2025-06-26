using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingTIcket.Models2;

namespace MovieBookingTIcket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheatreController : ControllerBase
    {
        private readonly DBContextTest _context;

        public TheatreController(DBContextTest context)
        {
            _context = context;
        }

        // GET: api/Theatre
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Theatre>>> GetTheatres()
        {
            var theatres = await _context.Theatres
                .Include(t => t.Screens)
                .ToListAsync();
            return Ok(theatres);
        }

        // GET: api/Theatre/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theatre>> GetTheatre(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Theatre ID cannot be null or empty");
            }

            var theatre = await _context.Theatres
                .Include(t => t.Screens)
                .FirstOrDefaultAsync(m => m.TheatreId == id);

            if (theatre == null)
            {
                return NotFound($"Theatre with ID {id} not found");
            }

            return Ok(theatre);
        }

        private bool TheatreExists(string id)
        {
            return _context.Theatres.Any(e => e.TheatreId == id);
        }
    }
}
