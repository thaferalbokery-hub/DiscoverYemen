using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;

namespace DiscoverYemen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "لوحة التحكم";
            ViewBag.TotalGovernorates = await _context.Governorates.CountAsync();
            ViewBag.TotalAttractions = await _context.Attractions.CountAsync();
            ViewBag.TotalHotels = await _context.Hotels.CountAsync();
            ViewBag.TotalRestaurants = await _context.Restaurants.CountAsync();
            ViewBag.TotalEvents = await _context.Events.CountAsync();
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();
            ViewBag.TotalReviews = await _context.Reviews.CountAsync();

            return View();
        }
    }
}