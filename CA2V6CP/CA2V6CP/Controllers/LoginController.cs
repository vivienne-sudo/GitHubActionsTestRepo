using CA2V6CP.Data;
using CA2V6CP.Models;
using Microsoft.AspNetCore.Mvc;

namespace CA2V6CP.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email and password are valid
                var user = _context.Registers.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    // Set the user as authenticated
                    HttpContext.Session.SetString("UserId", user.Email);

                    // Redirect to the logged in home page
                    return RedirectToAction("Index", "LoggedInHome");
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid email or password.");
                }
            }

            return View(model);
        }

    }
}
