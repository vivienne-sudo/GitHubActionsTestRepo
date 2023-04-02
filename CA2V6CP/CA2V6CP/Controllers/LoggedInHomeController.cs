using Microsoft.AspNetCore.Mvc;

namespace CA2V6CP.Controllers
{
    public class LoggedInHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
