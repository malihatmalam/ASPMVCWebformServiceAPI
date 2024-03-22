using Microsoft.AspNetCore.Mvc;

namespace MyRESTServices.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
