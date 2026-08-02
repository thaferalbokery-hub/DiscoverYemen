using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;
using DiscoverYemen.Models;

namespace DiscoverYemen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GovernoratesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GovernoratesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "إدارة المحافظات";
            var governorates = await _context.Governorates.ToListAsync();
            return View(governorates);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "إضافة محافظة";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Governorate governorate)
        {
            if (ModelState.IsValid)
            {
                _context.Governorates.Add(governorate);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم إضافة المحافظة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Title = "إضافة محافظة";
            return View(governorate);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var governorate = await _context.Governorates.FindAsync(id);
            if (governorate == null) return NotFound();
            ViewBag.Title = "تعديل المحافظة";
            return View(governorate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Governorate governorate)
        {
            if (ModelState.IsValid)
            {
                _context.Governorates.Update(governorate);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم تعديل المحافظة بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Title = "تعديل المحافظة";
            return View(governorate);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var governorate = await _context.Governorates.FindAsync(id);
            if (governorate == null) return NotFound();
            ViewBag.Title = "حذف المحافظة";
            return View(governorate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var governorate = await _context.Governorates.FindAsync(id);
            if (governorate != null)
            {
                _context.Governorates.Remove(governorate);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف المحافظة بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}