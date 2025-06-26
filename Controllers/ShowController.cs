using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingTIcket.Models2;

namespace MovieBookingTIcket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowController : ControllerBase
    {
        private readonly DBContextTest _context;

        public ShowController(DBContextTest context)
        {
            _context = context;
        }

        // GET: api/Show
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Show>>> GetShows()
        {
            var shows = await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .ToListAsync();
            return Ok(shows);
        }

        // GET: api/Show/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Show>> GetShow(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Show ID cannot be null or empty");
            }

            var show = await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .FirstOrDefaultAsync(m => m.ShowId == id);

            if (show == null)
            {
                return NotFound($"Show with ID {id} not found");
            }

            return Ok(show);
        }

        private bool ShowExists(string id)
        {
            return _context.Shows.Any(e => e.ShowId == id);
        }
    }
}
