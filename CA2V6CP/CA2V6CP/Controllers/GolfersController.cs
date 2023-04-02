using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CA2V6CP.Data;
using CA2V6CP.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CA2V6CP.Controllers
{
    public class GolfersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GolfersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string gender, int? handicapMin, int? handicapMax)
        {
            var golfers = _context.Golfers.AsQueryable();

            // Filter by gender
            if (!string.IsNullOrEmpty(gender))
            {
                golfers = golfers.Where(g => g.Sex == gender);
            }

            // Filter by handicap
            if (handicapMin.HasValue)
            {
                golfers = golfers.Where(g => g.Handicap >= handicapMin);
            }

            if (handicapMax.HasValue)
            {
                golfers = golfers.Where(g => g.Handicap <= handicapMax);
            }

            // Sort by name or handicap
            var sort = HttpContext.Request.Query["sort"].FirstOrDefault();
            switch (sort)
            {
                case "nameDesc":
                    golfers = golfers.OrderByDescending(g => g.Name);
                    break;
                case "handicapAsc":
                    golfers = golfers.OrderBy(g => g.Handicap);
                    break;
                case "handicapDesc":
                    golfers = golfers.OrderByDescending(g => g.Handicap);
                    break;
                default:
                    golfers = golfers.OrderBy(g => g.Name);
                    break;
            }

            return View(await golfers.ToListAsync());
        }

        // GET: Golfers
        public async Task<IActionResult> IndexByGender(string sex)
        {
            var golfers = _context.Golfers.AsQueryable();

            if (!string.IsNullOrEmpty(sex))
            {
                golfers = golfers.Where(g => g.Sex == sex);
            }

            return View(await golfers.ToListAsync());
        }

        public async Task<IActionResult> IndexByHandicapBelow10(int? handicapMax)
        {
            var golfers = _context.Golfers.AsQueryable();

            if (handicapMax.HasValue)
            {
                golfers = golfers.Where(g => g.Handicap < 10);
            }

            return View(await golfers.ToListAsync());
        }

        public async Task<IActionResult> IndexByHandicap(int? handicapMin, int? handicapMax)
        {
            var golfers = _context.Golfers.AsQueryable();

            if (handicapMin.HasValue)
            {
                golfers = golfers.Where(g => g.Handicap >= handicapMin);
            }

            if (handicapMax.HasValue)
            {
                golfers = golfers.Where(g => g.Handicap <= handicapMax);
            }

            return View(await golfers.ToListAsync());
        }

        // GET: Golfers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var golfer = await _context.Golfers
                .FirstOrDefaultAsync(m => m.MembershipNumber == id);

            if (golfer == null)
            {
                return NotFound();
            }

            return View(golfer);
        }

        // GET: Golfers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Golfers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipNumber,Name,Email,Sex,Handicap")] Golfer golfer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(golfer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(golfer);
        }

        // GET: Golfers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var golfer = await _context.Golfers.FindAsync(id);

            if (golfer == null)
            {
                return NotFound();
            }

            return View(golfer);
        }

        // POST: Golfers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembershipNumber,Name,Email,Sex,Handicap")] Golfer golfer)
        {
            if (id != golfer.MembershipNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(golfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GolferExists(golfer.MembershipNumber))
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

            return View(golfer);
        }

        // GET: Golfers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var golfer = await _context.Golfers
                .FirstOrDefaultAsync(m => m.MembershipNumber == id);

            if (golfer == null)
            {
                return NotFound();
            }

            return View(golfer);
        }

        // POST: Golfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var golfer = await _context.Golfers.FindAsync(id);
            _context.Golfers.Remove(golfer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GolferExists(int id)
        {
            return _context.Golfers.Any(e => e.MembershipNumber == id);
        }
    }
}

