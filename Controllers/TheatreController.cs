using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBE.Models2;

namespace MovieBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            try
            {
                var theatres = await _context.Theatres
                    .Include(t => t.Screens)
                    .ToListAsync();
                return Ok(theatres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving theatres.", error = ex.Message });
            }
        }

        // GET: api/Theatre/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theatre>> GetTheatre(string id)
        {
            try
            {
                var theatre = await _context.Theatres
                    .Include(t => t.Screens)
                    .FirstOrDefaultAsync(t => t.TheatreId == id);

                if (theatre == null)
                {
                    return NotFound(new { message = $"Theatre with ID {id} not found." });
                }

                return Ok(theatre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the theatre.", error = ex.Message });
            }
        }

        // PUT: api/Theatre/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTheatre(string id, Theatre theatre)
        {
            if (id != theatre.TheatreId)
            {
                return BadRequest(new { message = "Theatre ID mismatch." });
            }

            _context.Entry(theatre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Theatre updated successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TheatreExists(id))
                {
                    return NotFound(new { message = $"Theatre with ID {id} not found." });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the theatre.", error = ex.Message });
            }
        }

        // POST: api/Theatre
        [HttpPost]
        public async Task<ActionResult<Theatre>> PostTheatre(Theatre theatre)
        {
            try
            {
                if (TheatreExists(theatre.TheatreId))
                {
                    return Conflict(new { message = $"Theatre with ID {theatre.TheatreId} already exists." });
                }

                _context.Theatres.Add(theatre);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTheatre", new { id = theatre.TheatreId }, theatre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the theatre.", error = ex.Message });
            }
        }

        // DELETE: api/Theatre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheatre(string id)
        {
            try
            {
                var theatre = await _context.Theatres.FindAsync(id);
                if (theatre == null)
                {
                    return NotFound(new { message = $"Theatre with ID {id} not found." });
                }

                _context.Theatres.Remove(theatre);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Theatre deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the theatre.", error = ex.Message });
            }
        }

        private bool TheatreExists(string id)
        {
            return _context.Theatres.Any(e => e.TheatreId == id);
        }
    }
}
