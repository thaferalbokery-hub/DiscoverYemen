using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DiscoverYemen.Models;
using DiscoverYemen.Services;
using DiscoverYemen.ViewModels;

namespace DiscoverYemen.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly BookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingsController(BookingService bookingService, UserManager<ApplicationUser> userManager)
        {
            _bookingService = bookingService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "حجوزاتي";
            var userId = _userManager.GetUserId(User);
            var bookings = await _bookingService.GetUserBookingsAsync(userId!);
            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                await _bookingService.CreateBookingAsync(userId!, model);
                TempData["Success"] = "تم إنشاء الحجز بنجاح";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "حدث خطأ في إنشاء الحجز";
            return RedirectToAction("Index");
        }
    }
}