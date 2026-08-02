using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;

namespace DiscoverYemen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "اكتشف اليمن - الرئيسية";
            ViewBag.TotalGovernorates = await _context.Governorates.CountAsync();
            ViewBag.TotalAttractions = await _context.Attractions.CountAsync();
            ViewBag.TotalHotels = await _context.Hotels.CountAsync();

            var latestAttractions = await _context.Attractions
                .Include(a => a.Governorate)
                .OrderByDescending(a => a.Id)
                .Take(6)
                .ToListAsync();

            return View(latestAttractions);
        }

        public IActionResult Error()
        {
            ViewBag.Title = "خطأ";
            return View();
        }
    }
}