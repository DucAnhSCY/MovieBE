using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingTIcket.Models2;

namespace MovieBookingTIcket.Controllers
{
    public class UserController : Controller
    {
        private readonly DBContextTest _context;

        public UserController(DBContextTest context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _context.WebUsers.ToListAsync();
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.WebUsers
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Show)
                .ThenInclude(s => s.Movie)
                .FirstOrDefaultAsync(m => m.WebUserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WebUserId,FirstName,LastName,EmailId,Age,PhoneNumber")] WebUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.WebUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("WebUserId,FirstName,LastName,EmailId,Age,PhoneNumber")] WebUser user)
        {
            if (id != user.WebUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.WebUserId))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.WebUsers
                .FirstOrDefaultAsync(m => m.WebUserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.WebUsers.FindAsync(id);
            if (user != null)
            {
                _context.WebUsers.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.WebUsers.Any(e => e.WebUserId == id);
        }
    }
}
