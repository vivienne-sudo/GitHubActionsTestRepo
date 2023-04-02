using CA2V6CP.Data;
using CA2V6CP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CA2V6CP.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessRegistration(Register registration)
        {
            if (ModelState.IsValid)
            {
                // Check if the email is valid
                var validEmailDomains = new List<string> { "atu.com" };
                var emailDomain = registration.Email.Split('@')[1];
                if (!validEmailDomains.Contains(emailDomain))
                {
                    ModelState.AddModelError("Email", "You must use a valid company email address.");
                }
                else
                {
                    // Check if the email is already registered
                    if (_context.Registers.Any(r => r.Email == registration.Email))
                    {
                        ModelState.AddModelError("Email", "This email address is already registered.");
                    }
                    else
                    {
                        // Check if ConfirmPassword is set
                        if (string.IsNullOrEmpty(registration.ConfirmPassword))
                        {
                            ModelState.AddModelError("ConfirmPassword", "Confirm password is required.");
                        }
                        else
                        {
                            // Register the user
                            var register = new Register { Email = registration.Email, Password = registration.Password, ConfirmPassword = registration.ConfirmPassword };
                            _context.Registers.Add(register);
                            _context.SaveChanges();

                            // Set registration confirmation message
                            ViewBag.Message = "You have successfully registered! Please login to continue.";

                            // Redirect to login page
                            return RedirectToAction("RegisterConfirmation");
                        }
                    }
                }
            }

            // If we got this far, there was a problem with registration
            return View("Register", registration);
        }
        public IActionResult RegisterConfirmation()
        {
            // Display registration confirmation message
            ViewBag.Message = (string)ViewBag.Message ?? "Registration successful!";
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

    }
}