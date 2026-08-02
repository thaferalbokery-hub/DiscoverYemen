using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;
using DiscoverYemen.Models;

namespace DiscoverYemen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "إدارة المطاعم";
            var restaurants = await _context.Restaurants.Include(r => r.Governorate).ToListAsync();
            return View(restaurants);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "إضافة مطعم";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم إضافة المطعم بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Title = "إضافة مطعم";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View(restaurant);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return NotFound();
            ViewBag.Title = "تعديل المطعم";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Restaurants.Update(restaurant);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم تعديل المطعم بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Title = "تعديل المطعم";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View(restaurant);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var restaurant = await _context.Restaurants.Include(r => r.Governorate).FirstOrDefaultAsync(r => r.Id == id);
            if (restaurant == null) return NotFound();
            ViewBag.Title = "حذف المطعم";
            return View(restaurant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف المطعم بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}