using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;
using DiscoverYemen.Models;

namespace DiscoverYemen.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "المفضلة";
            var userId = _userManager.GetUserId(User);

            var favorites = await _context.Favorites
                .Include(f => f.Attraction)
                    .ThenInclude(a => a.Governorate)
                .Where(f => f.UserId == userId)
                .ToListAsync();

            return View(favorites);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int attractionId)
        {
            var userId = _userManager.GetUserId(User);

            var exists = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.AttractionId == attractionId);

            if (!exists)
            {
                var favorite = new Favorite
                {
                    UserId = userId!,
                    AttractionId = attractionId,
                    DateAdded = DateTime.UtcNow
                };
                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Attractions", new { id = attractionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int attractionId)
        {
            var userId = _userManager.GetUserId(User);

            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.AttractionId == attractionId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}