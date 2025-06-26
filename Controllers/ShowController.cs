using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBE.Models2;

namespace MovieBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly DBContextTest _context;

        public ShowController(DBContextTest context)
        {
            _context = context;
        }

        // GET: api/Show
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetShows()
        {
            try
            {
                var shows = await _context.Shows
                    .Include(s => s.Movie)
                    .Include(s => s.Screen)
                    .ThenInclude(sc => sc!.Theatre)
                    .Select(s => new
                    {
                        s.ShowId,
                        s.ShowTime,
                        s.ShowDate,
                        s.SeatsRemainingGold,
                        s.SeatsRemainingSilver,
                        s.ClassCostGold,
                        s.ClassCostSilver,
                        s.ScreenId,
                        s.MovieId,
                        Movie = s.Movie != null ? new
                        {
                            s.Movie.MovieId,
                            s.Movie.Name,
                            s.Movie.Language,
                            s.Movie.Genre,
                            s.Movie.TargetAudience,
                            s.Movie.Duration,
                            s.Movie.ReleaseDate,
                            s.Movie.PosterUrl,
                            s.Movie.Description
                        } : null,
                        Screen = s.Screen != null ? new
                        {
                            s.Screen.ScreenId,
                            s.Screen.NoOfSeatsGold,
                            s.Screen.NoOfSeatsSilver,
                            Theatre = s.Screen.Theatre != null ? new
                            {
                                s.Screen.Theatre.TheatreId,
                                s.Screen.Theatre.NameOfTheatre,
                                s.Screen.Theatre.Area
                            } : null
                        } : null
                    })
                    .ToListAsync();

                return Ok(shows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving shows.", error = ex.Message });
            }
        }

        // GET: api/Show/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetShow(string id)
        {
            try
            {
                var show = await _context.Shows
                    .Include(s => s.Movie)
                    .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                    .Where(s => s.ShowId == id)
                    .Select(s => new
                    {
                        s.ShowId,
                        s.ShowTime,
                        s.ShowDate,
                        s.SeatsRemainingGold,
                        s.SeatsRemainingSilver,
                        s.ClassCostGold,
                        s.ClassCostSilver,
                        s.ScreenId,
                        s.MovieId,
                        Movie = new
                        {
                            s.Movie.MovieId,
                            s.Movie.Name,
                            s.Movie.Language,
                            s.Movie.Genre,
                            s.Movie.TargetAudience,
                            s.Movie.Duration,
                            s.Movie.ReleaseDate,
                            s.Movie.PosterUrl,
                            s.Movie.Description
                        },
                        Screen = new
                        {
                            s.Screen.ScreenId,
                            s.Screen.NoOfSeatsGold,
                            s.Screen.NoOfSeatsSilver,
                            Theatre = new
                            {
                                s.Screen.Theatre.TheatreId,
                                s.Screen.Theatre.NameOfTheatre,
                                s.Screen.Theatre.Area
                            }
                        }
                    })
                    .FirstOrDefaultAsync();

                if (show == null)
                {
                    return NotFound(new { message = $"Show with ID {id} not found." });
                }

                return Ok(show);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the show.", error = ex.Message });
            }
        }

        // GET: api/Show/movie/5
        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetShowsByMovie(string movieId)
        {
            try
            {
                var shows = await _context.Shows
                    .Include(s => s.Movie)
                    .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                    .Where(s => s.MovieId == movieId)
                    .Select(s => new
                    {
                        s.ShowId,
                        s.ShowTime,
                        s.ShowDate,
                        s.SeatsRemainingGold,
                        s.SeatsRemainingSilver,
                        s.ClassCostGold,
                        s.ClassCostSilver,
                        s.ScreenId,
                        s.MovieId,
                        Screen = new
                        {
                            s.Screen.ScreenId,
                            s.Screen.NoOfSeatsGold,
                            s.Screen.NoOfSeatsSilver,
                            Theatre = new
                            {
                                s.Screen.Theatre.TheatreId,
                                s.Screen.Theatre.NameOfTheatre,
                                s.Screen.Theatre.Area
                            }
                        }
                    })
                    .ToListAsync();

                return Ok(shows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving shows for the movie.", error = ex.Message });
            }
        }

        // PUT: api/Show/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShow(string id, Show show)
        {
            if (id != show.ShowId)
            {
                return BadRequest(new { message = "Show ID mismatch." });
            }

            _context.Entry(show).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Show updated successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowExists(id))
                {
                    return NotFound(new { message = $"Show with ID {id} not found." });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the show.", error = ex.Message });
            }
        }

        // POST: api/Show
        [HttpPost]
        public async Task<ActionResult<Show>> PostShow(Show show)
        {
            try
            {
                if (ShowExists(show.ShowId))
                {
                    return Conflict(new { message = $"Show with ID {show.ShowId} already exists." });
                }

                _context.Shows.Add(show);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetShow", new { id = show.ShowId }, show);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the show.", error = ex.Message });
            }
        }

        // DELETE: api/Show/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(string id)
        {
            try
            {
                var show = await _context.Shows.FindAsync(id);
                if (show == null)
                {
                    return NotFound(new { message = $"Show with ID {id} not found." });
                }

                _context.Shows.Remove(show);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Show deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the show.", error = ex.Message });
            }
        }

        private bool ShowExists(string id)
        {
            return _context.Shows.Any(e => e.ShowId == id);
        }
    }
}
