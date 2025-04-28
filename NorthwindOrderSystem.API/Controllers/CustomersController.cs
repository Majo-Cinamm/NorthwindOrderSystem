using Microsoft.AspNetCore.Mvc;

namespace NorthwindOrderSystem.API.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
