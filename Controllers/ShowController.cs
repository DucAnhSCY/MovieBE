using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieBookingTIcket.Models2;

namespace MovieBookingTIcket.Controllers
{
    public class ShowController : Controller
    {
        private readonly DBContextTest _context;

        public ShowController(DBContextTest context)
        {
            _context = context;
        }

        // GET: Show
        public async Task<IActionResult> Index()
        {
            var shows = await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .ToListAsync();
            return View(shows);
        }

        // GET: Show/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .Include(s => s.Bookings)
                .FirstOrDefaultAsync(m => m.ShowId == id);

            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }

        // GET: Show/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Name");
            ViewData["ScreenId"] = new SelectList(_context.Screens
                .Include(s => s.Theatre)
                .Select(s => new {
                    s.ScreenId,
                    DisplayText = $"{s.Theatre.NameOfTheatre} - Screen {s.ScreenId}"
                }), "ScreenId", "DisplayText");
            return View();
        }

        // POST: Show/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowId,ShowDate,ShowTime,SeatsRemainingGold,SeatsRemainingSilver,ClassCostGold,ClassCostSilver,MovieId,ScreenId")] Show show)
        {
            if (ModelState.IsValid)
            {
                _context.Add(show);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Name", show.MovieId);
            ViewData["ScreenId"] = new SelectList(_context.Screens
                .Include(s => s.Theatre)
                .Select(s => new {
                    s.ScreenId,
                    DisplayText = $"{s.Theatre.NameOfTheatre} - Screen {s.ScreenId}"
                }), "ScreenId", "DisplayText", show.ScreenId);
            return View(show);
        }

        // GET: Show/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows.FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }
            
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Name", show.MovieId);
            ViewData["ScreenId"] = new SelectList(_context.Screens
                .Include(s => s.Theatre)
                .Select(s => new {
                    s.ScreenId,
                    DisplayText = $"{s.Theatre.NameOfTheatre} - Screen {s.ScreenId}"
                }), "ScreenId", "DisplayText", show.ScreenId);
            return View(show);
        }

        // POST: Show/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ShowId,ShowDate,ShowTime,SeatsRemainingGold,SeatsRemainingSilver,ClassCostGold,ClassCostSilver,MovieId,ScreenId")] Show show)
        {
            if (id != show.ShowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(show);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowExists(show.ShowId))
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
            
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Name", show.MovieId);
            ViewData["ScreenId"] = new SelectList(_context.Screens
                .Include(s => s.Theatre)
                .Select(s => new {
                    s.ScreenId,
                    DisplayText = $"{s.Theatre.NameOfTheatre} - Screen {s.ScreenId}"
                }), "ScreenId", "DisplayText", show.ScreenId);
            return View(show);
        }

        // GET: Show/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theatre)
                .FirstOrDefaultAsync(m => m.ShowId == id);
            
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }

        // POST: Show/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show != null)
            {
                _context.Shows.Remove(show);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowExists(string id)
        {
            return _context.Shows.Any(e => e.ShowId == id);
        }
    }
}
