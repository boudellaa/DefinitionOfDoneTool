using Microsoft.AspNetCore.Mvc;
using DoneTool.Data;
using DoneTool.Models;
using Microsoft.EntityFrameworkCore;

namespace DoneTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly DoneToolContext _context;

        public HomeController(DoneToolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var checksItem = await _context.Checks
                                              .Where(ci => ci.ID == id)
                                              .Select(ci => ci.Item)
                                              .ToListAsync();

            if (checksItem == null || !checksItem.Any())
            {
                return NotFound(); 
            }

            return View(checksItem);
        }
    }

}
