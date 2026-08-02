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
    public class HotelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "إدارة الفنادق";
            var hotels = await _context.Hotels.Include(h => h.Governorate).ToListAsync();
            return View(hotels);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "إضافة فندق";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Hotels.Add(hotel);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم إضافة الفندق بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Title = "إضافة فندق";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View(hotel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return NotFound();
            ViewBag.Title = "تعديل الفندق";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Hotels.Update(hotel);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم تعديل الفندق بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Title = "تعديل الفندق";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View(hotel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _context.Hotels.Include(h => h.Governorate).FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null) return NotFound();
            ViewBag.Title = "حذف الفندق";
            return View(hotel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف الفندق بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}