using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DiscoverYemen.Data;
using DiscoverYemen.Models;

namespace DiscoverYemen.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int attractionId, string comment, int rating)
        {
            if (string.IsNullOrWhiteSpace(comment) || rating < 1 || rating > 5)
            {
                TempData["Error"] = "يرجى ملء جميع الحقول بشكل صحيح";
                return RedirectToAction("Details", "Attractions", new { id = attractionId });
            }

            var userId = _userManager.GetUserId(User);

            var review = new Review
            {
                UserId = userId!,
                AttractionId = attractionId,
                Comment = comment,
                Rating = rating,
                DateCreated = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إضافة التقييم بنجاح";
            return RedirectToAction("Details", "Attractions", new { id = attractionId });
        }
    }
}