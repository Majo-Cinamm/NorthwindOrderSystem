using Microsoft.AspNetCore.Mvc;

namespace NorthwindOrderSystem.API.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
