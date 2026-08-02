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
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "إدارة الفعاليات";
            var events = await _context.Events.Include(e => e.Governorate).ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "إضافة فعالية";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event evt)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(evt);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم إضافة الفعالية بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Title = "إضافة فعالية";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View(evt);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null) return NotFound();
            ViewBag.Title = "تعديل الفعالية";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View(evt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Event evt)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Update(evt);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم تعديل الفعالية بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Title = "تعديل الفعالية";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            return View(evt);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var evt = await _context.Events.Include(e => e.Governorate).FirstOrDefaultAsync(e => e.Id == id);
            if (evt == null) return NotFound();
            ViewBag.Title = "حذف الفعالية";
            return View(evt);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt != null)
            {
                _context.Events.Remove(evt);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف الفعالية بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}