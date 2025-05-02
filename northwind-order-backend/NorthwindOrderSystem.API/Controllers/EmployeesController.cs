using Microsoft.AspNetCore.Mvc;
using NorthwindOrderSystem.Infrastructure.Data;

namespace NorthwindOrderSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindDbContext _context;

        public EmployeesController(NorthwindDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _context.Employees
                .Where(e => e.EmployeeId != null)
                .Select(e => new
                {
                    EmployeeId = e.EmployeeId.Value,
                    FullName = e.FirstName + " " + e.LastName,
                    e.Title
                })
                .ToList();

            return Ok(employees);
        }
    }
}
