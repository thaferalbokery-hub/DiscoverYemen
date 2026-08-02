using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;

namespace DiscoverYemen.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? status)
        {
            ViewBag.Title = "الفعاليات";
            ViewData["CurrentStatus"] = status;

            var query = _context.Events
                .Include(e => e.Governorate)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(e => e.Status == status);
            }

            var events = await query.OrderByDescending(e => e.StartDate).ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Details(int id)
        {
            var evt = await _context.Events
                .Include(e => e.Governorate)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evt == null)
                return NotFound();

            ViewBag.Title = evt.Name;
            return View(evt);
        }
    }
}