using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingTIcket.Models2;

namespace MovieBookingTIcket.Controllers
{
    public class TheatreController : Controller
    {
        private readonly DBContextTest _context;

        public TheatreController(DBContextTest context)
        {
            _context = context;
        }

        // GET: Theatre
        public async Task<IActionResult> Index()
        {
            var theatres = await _context.Theatres
                .Include(t => t.Screens)
                .ToListAsync();
            return View(theatres);
        }

        // GET: Theatre/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theatre = await _context.Theatres
                .Include(t => t.Screens)
                .ThenInclude(s => s.Shows)
                .ThenInclude(sh => sh.Movie)
                .FirstOrDefaultAsync(m => m.TheatreId == id);

            if (theatre == null)
            {
                return NotFound();
            }

            return View(theatre);
        }

        // GET: Theatre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Theatre/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TheatreId,NameOfTheatre,NoOfScreens,Area")] Theatre theatre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(theatre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theatre);
        }

        // GET: Theatre/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theatre = await _context.Theatres.FindAsync(id);
            if (theatre == null)
            {
                return NotFound();
            }
            return View(theatre);
        }

        // POST: Theatre/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TheatreId,NameOfTheatre,NoOfScreens,Area")] Theatre theatre)
        {
            if (id != theatre.TheatreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theatre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheatreExists(theatre.TheatreId))
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
            return View(theatre);
        }

        // GET: Theatre/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theatre = await _context.Theatres
                .FirstOrDefaultAsync(m => m.TheatreId == id);
            if (theatre == null)
            {
                return NotFound();
            }

            return View(theatre);
        }

        // POST: Theatre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var theatre = await _context.Theatres.FindAsync(id);
            if (theatre != null)
            {
                _context.Theatres.Remove(theatre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TheatreExists(string id)
        {
            return _context.Theatres.Any(e => e.TheatreId == id);
        }
    }
}
