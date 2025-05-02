using Microsoft.AspNetCore.Mvc;
using NorthwindOrderSystem.Core.Entities;
using NorthwindOrderSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace NorthwindOrderSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindDbContext _context;

        public ProductsController(NorthwindDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
    }
}
