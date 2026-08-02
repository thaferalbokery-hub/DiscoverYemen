using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;

namespace DiscoverYemen.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? governorateId)
        {
            ViewBag.Title = "الفنادق";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name", governorateId);
            ViewData["CurrentGovernorate"] = governorateId;

            var query = _context.Hotels
                .Include(h => h.Governorate)
                .AsQueryable();

            if (governorateId.HasValue)
            {
                query = query.Where(h => h.GovernorateId == governorateId.Value);
            }

            var hotels = await query.ToListAsync();
            return View(hotels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Governorate)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
                return NotFound();

            ViewBag.Title = hotel.Name;
            return View(hotel);
        }
    }
}