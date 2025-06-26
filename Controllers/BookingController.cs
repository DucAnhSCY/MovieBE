using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieBookingTIcket.Models2;

namespace MovieBookingTIcket.Controllers
{
    public class BookingController : Controller
    {
        private readonly DBContextTest _context;

        public BookingController(DBContextTest context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Show)
                .Include(b => b.User)
                .ToListAsync();
            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Show)
                .ThenInclude(s => s.Movie)
                .Include(b => b.Show)
                .ThenInclude(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .Include(b => b.User)
                .Include(b => b.Tickets)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            ViewData["ShowId"] = new SelectList(_context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .Select(s => new {
                    s.ShowId,
                    DisplayText = $"{s.Movie.Name} - {s.Screen.Theatre.NameOfTheatre} - {s.ShowDate} {s.ShowTime}"
                }), "ShowId", "DisplayText");
            
            ViewData["UserId"] = new SelectList(_context.WebUsers, "WebUserId", "FirstName");
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,NoOfTickets,TotalCost,CardNumber,NameOnCard,UserId,ShowId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ShowId"] = new SelectList(_context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .Select(s => new {
                    s.ShowId,
                    DisplayText = $"{s.Movie.Name} - {s.Screen.Theatre.NameOfTheatre} - {s.ShowDate} {s.ShowTime}"
                }), "ShowId", "DisplayText", booking.ShowId);
            
            ViewData["UserId"] = new SelectList(_context.WebUsers, "WebUserId", "FirstName", booking.UserId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            
            ViewData["ShowId"] = new SelectList(_context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .Select(s => new {
                    s.ShowId,
                    DisplayText = $"{s.Movie.Name} - {s.Screen.Theatre.NameOfTheatre} - {s.ShowDate} {s.ShowTime}"
                }), "ShowId", "DisplayText", booking.ShowId);
            
            ViewData["UserId"] = new SelectList(_context.WebUsers, "WebUserId", "FirstName", booking.UserId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookingId,NoOfTickets,TotalCost,CardNumber,NameOnCard,UserId,ShowId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ShowId"] = new SelectList(_context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .Select(s => new {
                    s.ShowId,
                    DisplayText = $"{s.Movie.Name} - {s.Screen.Theatre.NameOfTheatre} - {s.ShowDate} {s.ShowTime}"
                }), "ShowId", "DisplayText", booking.ShowId);
            
            ViewData["UserId"] = new SelectList(_context.WebUsers, "WebUserId", "FirstName", booking.UserId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Show)
                .ThenInclude(s => s.Movie)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(string id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
