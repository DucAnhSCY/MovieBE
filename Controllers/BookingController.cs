using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBE.Models2;

namespace MovieBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly DBContextTest _context;

        public BookingController(DBContextTest context)
        {
            _context = context;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBookings()
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.User)
                    .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                    .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                    .ThenInclude(sc => sc!.Theatre)
                    .Include(b => b.Tickets)
                    .Select(b => new
                    {
                        b.BookingId,
                        b.NoOfTickets,
                        b.TotalCost,
                        b.CardNumber,
                        b.NameOnCard,
                        b.BookingDate,
                        b.BookingStatus,
                        b.UserId,
                        b.ShowId,
                        User = b.User != null ? new
                        {
                            b.User.WebUserId,
                            b.User.FirstName,
                            b.User.LastName,
                            b.User.EmailId,
                            b.User.PhoneNumber
                        } : null,
                        Show = b.Show != null ? new
                        {
                            b.Show.ShowId,
                            b.Show.ShowTime,
                            b.Show.ShowDate,
                            Movie = b.Show.Movie != null ? new
                            {
                                b.Show.Movie.MovieId,
                                b.Show.Movie.Name,
                                b.Show.Movie.Language,
                                b.Show.Movie.Genre
                            } : null,
                            Theatre = b.Show.Screen != null && b.Show.Screen.Theatre != null ? new
                            {
                                b.Show.Screen.Theatre.TheatreId,
                                b.Show.Screen.Theatre.NameOfTheatre,
                                b.Show.Screen.Theatre.Area
                            } : null,
                            Screen = b.Show.Screen != null ? new
                            {
                                b.Show.Screen.ScreenId,
                                b.Show.Screen.NoOfSeatsGold,
                                b.Show.Screen.NoOfSeatsSilver
                            } : null
                        } : null,
                        Tickets = b.Tickets.Select(t => new
                        {
                            t.TicketId,
                            t.Class,
                            t.SeatNumber,
                            t.Price
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving bookings.", error = ex.Message });
            }
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetBooking(string id)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.User)
                    .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                    .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                    .ThenInclude(sc => sc!.Theatre)
                    .Include(b => b.Tickets)
                    .Where(b => b.BookingId == id)
                    .Select(b => new
                    {
                        b.BookingId,
                        b.NoOfTickets,
                        b.TotalCost,
                        b.CardNumber,
                        b.NameOnCard,
                        b.BookingDate,
                        b.BookingStatus,
                        b.UserId,
                        b.ShowId,
                        User = b.User != null ? new
                        {
                            b.User.WebUserId,
                            b.User.FirstName,
                            b.User.LastName,
                            b.User.EmailId,
                            b.User.PhoneNumber
                        } : null,
                        Show = b.Show != null ? new
                        {
                            b.Show.ShowId,
                            b.Show.ShowTime,
                            b.Show.ShowDate,
                            Movie = b.Show.Movie != null ? new
                            {
                                b.Show.Movie.MovieId,
                                b.Show.Movie.Name,
                                b.Show.Movie.Language,
                                b.Show.Movie.Genre
                            } : null,
                            Theatre = b.Show.Screen != null && b.Show.Screen.Theatre != null ? new
                            {
                                b.Show.Screen.Theatre.TheatreId,
                                b.Show.Screen.Theatre.NameOfTheatre,
                                b.Show.Screen.Theatre.Area
                            } : null,
                            Screen = b.Show.Screen != null ? new
                            {
                                b.Show.Screen.ScreenId,
                                b.Show.Screen.NoOfSeatsGold,
                                b.Show.Screen.NoOfSeatsSilver
                            } : null
                        } : null,
                        Tickets = b.Tickets.Select(t => new
                        {
                            t.TicketId,
                            t.Class,
                            t.SeatNumber,
                            t.Price
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (booking == null)
                {
                    return NotFound(new { message = $"Booking with ID {id} not found." });
                }

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the booking.", error = ex.Message });
            }
        }

        // GET: api/Booking/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetBookingsByUser(string userId)
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                    .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                    .ThenInclude(sc => sc!.Theatre)
                    .Include(b => b.Tickets)
                    .Where(b => b.UserId == userId)
                    .Select(b => new
                    {
                        b.BookingId,
                        b.NoOfTickets,
                        b.TotalCost,
                        b.BookingDate,
                        b.BookingStatus,
                        b.ShowId,
                        Show = b.Show != null ? new
                        {
                            b.Show.ShowId,
                            b.Show.ShowTime,
                            b.Show.ShowDate,
                            Movie = b.Show.Movie != null ? new
                            {
                                b.Show.Movie.MovieId,
                                b.Show.Movie.Name,
                                b.Show.Movie.Language,
                                b.Show.Movie.Genre
                            } : null,
                            Theatre = b.Show.Screen != null && b.Show.Screen.Theatre != null ? new
                            {
                                b.Show.Screen.Theatre.TheatreId,
                                b.Show.Screen.Theatre.NameOfTheatre,
                                b.Show.Screen.Theatre.Area
                            } : null,
                            Screen = b.Show.Screen != null ? new
                            {
                                b.Show.Screen.ScreenId,
                                b.Show.Screen.NoOfSeatsGold,
                                b.Show.Screen.NoOfSeatsSilver
                            } : null
                        } : null,
                        Tickets = b.Tickets.Select(t => new
                        {
                            t.TicketId,
                            t.Class,
                            t.SeatNumber,
                            t.Price
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving user bookings.", error = ex.Message });
            }
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(string id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest(new { message = "Booking ID mismatch." });
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Booking updated successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound(new { message = $"Booking with ID {id} not found." });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the booking.", error = ex.Message });
            }
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<ActionResult<object>> PostBooking(Booking booking)
        {
            try
            {
                if (BookingExists(booking.BookingId))
                {
                    return Conflict(new { message = $"Booking with ID {booking.BookingId} already exists." });
                }

                // Set booking date if not provided
                if (booking.BookingDate == null)
                {
                    booking.BookingDate = DateTime.Now;
                }

                // Set default status if not provided
                if (string.IsNullOrEmpty(booking.BookingStatus))
                {
                    booking.BookingStatus = "Confirmed";
                }

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Return the created booking with related data
                var createdBooking = await _context.Bookings
                    .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                    .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                    .ThenInclude(sc => sc!.Theatre)
                    .Where(b => b.BookingId == booking.BookingId)
                    .Select(b => new
                    {
                        b.BookingId,
                        b.NoOfTickets,
                        b.TotalCost,
                        b.BookingDate,
                        b.BookingStatus,
                        b.UserId,
                        b.ShowId,
                        Show = b.Show != null ? new
                        {
                            b.Show.ShowId,
                            b.Show.ShowTime,
                            b.Show.ShowDate,
                            Movie = b.Show.Movie != null ? new
                            {
                                b.Show.Movie.MovieId,
                                b.Show.Movie.Name
                            } : null,
                            Theatre = b.Show.Screen != null && b.Show.Screen.Theatre != null ? new
                            {
                                b.Show.Screen.Theatre.NameOfTheatre,
                                b.Show.Screen.Theatre.Area
                            } : null
                        } : null
                    })
                    .FirstOrDefaultAsync();

                return CreatedAtAction("GetBooking", new { id = booking.BookingId }, createdBooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the booking.", error = ex.Message });
            }
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return NotFound(new { message = $"Booking with ID {id} not found." });
                }

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Booking deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the booking.", error = ex.Message });
            }
        }

        private bool BookingExists(string id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
