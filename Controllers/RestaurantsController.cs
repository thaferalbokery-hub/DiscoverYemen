using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;

namespace DiscoverYemen.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? governorateId)
        {
            ViewBag.Title = "المطاعم";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name", governorateId);
            ViewData["CurrentGovernorate"] = governorateId;

            var query = _context.Restaurants
                .Include(r => r.Governorate)
                .AsQueryable();

            if (governorateId.HasValue)
            {
                query = query.Where(r => r.GovernorateId == governorateId.Value);
            }

            var restaurants = await query.ToListAsync();
            return View(restaurants);
        }

        public async Task<IActionResult> Details(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.Governorate)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
                return NotFound();

            ViewBag.Title = restaurant.Name;
            return View(restaurant);
        }
    }
}