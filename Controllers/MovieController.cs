using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBE.Models2;

namespace MovieBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DBContextTest _context;

        public MovieController(DBContextTest context)
        {
            _context = context;
        }

        // GET: api/Movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            try
            {
                var movies = await _context.Movies.ToListAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving movies.", error = ex.Message });
            }
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(string id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);

                if (movie == null)
                {
                    return NotFound(new { message = $"Movie with ID {id} not found." });
                }

                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the movie.", error = ex.Message });
            }
        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(string id, Movie movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest(new { message = "Movie ID mismatch." });
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Movie updated successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound(new { message = $"Movie with ID {id} not found." });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the movie.", error = ex.Message });
            }
        }

        // POST: api/Movie
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            try
            {
                // Check if movie with same ID already exists
                if (MovieExists(movie.MovieId))
                {
                    return Conflict(new { message = $"Movie with ID {movie.MovieId} already exists." });
                }

                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMovie", new { id = movie.MovieId }, movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the movie.", error = ex.Message });
            }
        }

        // DELETE: api/Movie/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie == null)
                {
                    return NotFound(new { message = $"Movie with ID {id} not found." });
                }

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Movie deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the movie.", error = ex.Message });
            }
        }

        // GET: api/Movie/genres
        [HttpGet("genres")]
        public async Task<ActionResult<IEnumerable<string>>> GetGenres()
        {
            try
            {
                var genres = await _context.Movies
                    .Where(m => !string.IsNullOrEmpty(m.Genre))
                    .Select(m => m.Genre)
                    .Distinct()
                    .ToListAsync();

                return Ok(genres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving genres.", error = ex.Message });
            }
        }

        // GET: api/Movie/languages
        [HttpGet("languages")]
        public async Task<ActionResult<IEnumerable<string>>> GetLanguages()
        {
            try
            {
                var languages = await _context.Movies
                    .Where(m => !string.IsNullOrEmpty(m.Language))
                    .Select(m => m.Language)
                    .Distinct()
                    .ToListAsync();

                return Ok(languages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving languages.", error = ex.Message });
            }
        }

        private bool MovieExists(string id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
