using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CA2V6CP.Data;
using CA2V6CP.Services;
using Microsoft.AspNetCore.Authorization;
using CA2V6CP.Models; 

namespace CA2V6CP.Controllers;

public class TeeTimeBookingsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TeeTimeBookingsController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult GetTeeTimeBookings()
    {
        var bookings = _context.TeeTimeBookings.ToList();
        return Json(bookings);
    }


    // GET: TeeTimeBookings
    [AllowAnonymous]
    public async Task<IActionResult> Index(string playerName, bool reset = false)
    {
        if (reset)
        {
            return RedirectToAction(nameof(Index));
        }

        var today = DateTime.Today;
        var teeTimeBookings = await _context.TeeTimeBookings.Where(t => t.StartTime >= DateTime.Today).OrderBy(t => t.StartTime).ToListAsync();

        if (!string.IsNullOrEmpty(playerName))
        {
            teeTimeBookings = teeTimeBookings.Where(b =>
                b.Player1Name.Contains(playerName) ||
                b.Player2Name.Contains(playerName) ||
                b.Player3Name.Contains(playerName) ||
                b.Player4Name.Contains(playerName)).ToList();
        }

        return View(teeTimeBookings);
    }


    // GET: TeeTimeBookings/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.TeeTimeBookings == null)
        {
            return NotFound();
        }

        var teeTimeBooking = await _context.TeeTimeBookings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teeTimeBooking == null)
        {
            return NotFound();
        }

        return View(teeTimeBooking);
    }

    // GET: TeeTimeBookings/Create
    public IActionResult Create()
    {
        var golfers = _context.Golfers.ToList();
        if (golfers == null)
        {
            // Handle null list as needed
            return View("Error");
        }
        ViewBag.OpeningTime = new SeasonalOpeningHours().OpeningTime.ToString("HH:mm");
        ViewBag.GolfersSelectList = new SelectList(_context.Golfers, "MembershipNumber", "Name");
        ViewBag.Golfers = golfers;
        return View();
    }
    // POST: TeeTimeBookings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeeTimeBooking teeTimeBooking)
    {
        // Check if any of the selected golfers have already been booked for a tee time on the same day
        var bookedGolfers = await _context.TeeTimeBookings
            .Where(t => t.StartTime.Date == teeTimeBooking.StartTime.Date)
            .Select(t => new List<string> { t.Player1Name, t.Player2Name, t.Player3Name, t.Player4Name })
            .ToListAsync();

        if (bookedGolfers.Any(g => g.Contains(teeTimeBooking.Player1Name) ||
                                   g.Contains(teeTimeBooking.Player2Name) ||
                                   g.Contains(teeTimeBooking.Player3Name) ||
                                   g.Contains(teeTimeBooking.Player4Name)))
        {
            ModelState.AddModelError(string.Empty, "One or more of the selected golfers have already been booked for a tee time on the same day.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(teeTimeBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Repopulate the list of golfers
        ViewBag.Golfers = await _context.Golfers.ToListAsync();

        return View(teeTimeBooking);
    }



    // GET: TeeTimeBookings/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teeTimeBooking = await _context.TeeTimeBookings.FindAsync(id);
        if (teeTimeBooking == null)
        {
            return NotFound();
        }

        ViewBag.Golfers = _context.Golfers.ToList();
        return View(teeTimeBooking);
    }

    // POST: TeeTimeBookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,Player1Name,Player1Handicap,Player2Name,Player2Handicap,Player3Name,Player3Handicap,Player4Name,Player4Handicap")] TeeTimeBooking teeTimeBooking)
    {
        if (id != teeTimeBooking.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teeTimeBooking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeeTimeBookingExists(teeTimeBooking.Id))
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
        return View(teeTimeBooking);
    }


    // GET: TeeTimeBookings/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teeTimeBooking = await _context.TeeTimeBookings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teeTimeBooking == null)
        {
            return NotFound();
        }

        return View(teeTimeBooking);
    }

    // POST: TeeTimeBookings/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teeTimeBooking = await _context.TeeTimeBookings.FindAsync(id);
        _context.TeeTimeBookings.Remove(teeTimeBooking);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeeTimeBookingExists(int id)
    {
        return _context.TeeTimeBookings.Any(e => e.Id == id);
    }

    public IActionResult VerifyStartTime(DateTime startTime)
    {
        var openingHours = new SeasonalOpeningHours();
        var openingTime = openingHours.OpeningTime;
        var closingTime = openingHours.ClosingTime;


        if (startTime < DateTime.Now)
        {
            return Json("Booking start time must be in the future.");
        }
        // Check if the start time is within opening hours
        if (startTime < openingTime || startTime >= closingTime)
        {
            return Json("Bookings can only be made between " + openingTime.ToString("HH:mm") + " and " + closingTime.ToString("HH:mm") + ".");
        }

        // Check if the start time is a multiple of 15 minutes
        if (startTime.Minute % 15 != 0)
        {
            return Json("Bookings can only be made every 15 minutes.");
        }

        // Check if there are already 4 bookings in the same hour
        var bookingsInSameHour = _context.TeeTimeBookings
            .Where(b => b.StartTime.Year == startTime.Year && b.StartTime.Month == startTime.Month && b.StartTime.Day == startTime.Day && b.StartTime.Hour == startTime.TimeOfDay.Hours).Count();
        if (bookingsInSameHour >= 4)
        {
            return Json("This hour is fully booked.");
        }

        return Json(true);


    }

}

