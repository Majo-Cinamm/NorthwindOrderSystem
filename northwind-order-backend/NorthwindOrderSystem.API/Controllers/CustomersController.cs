using Microsoft.AspNetCore.Mvc;
using NorthwindOrderSystem.Infrastructure.Data;

namespace NorthwindOrderSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly NorthwindDbContext _context;

        public CustomersController(NorthwindDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _context.Customers
                .Select(c => new
                {
                    c.CustomerId,
                    c.ContactName,
                    c.CompanyName
                })
                .ToList();

            return Ok(customers);
        }
    }
}
