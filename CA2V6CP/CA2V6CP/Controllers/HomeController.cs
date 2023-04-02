using CA2V6CP.Data;
using CA2V6CP.Models;
using CA2V6CP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace CA2V6CP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index(string playerName)
        {
            var bookings = _context.TeeTimeBookings
                .Where(b => b.StartTime > DateTime.Now)
                .OrderBy(b => b.StartTime)
                .ToList();

            if (!string.IsNullOrEmpty(playerName))
            {
                bookings = bookings.Where(b => b.Player1Name.Contains(playerName) ||
                                               b.Player2Name.Contains(playerName) ||
                                               b.Player3Name.Contains(playerName) ||
                                               b.Player4Name.Contains(playerName))
                                   .ToList();
                ViewBag.CurrentFilter = playerName;
            }

            return View(bookings);
        }

        // Helper method to get the current season based on the current date
        private string GetSeason()
        {
            var month = DateTime.Now.Month;
            if (month >= 3 && month <= 5)
            {
                return "spring";
            }
            else if (month >= 6 && month <= 8)
            {
                return "summer";
            }
            else if (month >= 9 && month <= 11)
            {
                return "autumn";
            }
            else
            {
                return "winter";
            }
        }

    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
