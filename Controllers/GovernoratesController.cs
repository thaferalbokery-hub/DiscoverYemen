using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;

namespace DiscoverYemen.Controllers
{
    public class GovernoratesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GovernoratesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchTerm)
        {
            ViewBag.Title = "المحافظات";
            ViewData["CurrentSearch"] = searchTerm;

            var query = _context.Governorates.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(g => g.Name.Contains(searchTerm) || (g.Description != null && g.Description.Contains(searchTerm)));
            }

            var governorates = await query.ToListAsync();
            return View(governorates);
        }

        public async Task<IActionResult> Details(int id)
        {
            var governorate = await _context.Governorates
                .Include(g => g.Attractions)
                .Include(g => g.Hotels)
                .Include(g => g.Restaurants)
                .Include(g => g.Events)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (governorate == null)
                return NotFound();

            ViewBag.Title = governorate.Name;
            return View(governorate);
        }
    }
}