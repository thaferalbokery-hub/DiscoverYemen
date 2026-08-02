using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;
using DiscoverYemen.Models;
using DiscoverYemen.Services;
using DiscoverYemen.ViewModels;

namespace DiscoverYemen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AttractionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttractionService _attractionService;

        public AttractionsController(ApplicationDbContext context, AttractionService attractionService)
        {
            _context = context;
            _attractionService = attractionService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "إدارة المعالم";
            var attractions = await _context.Attractions
                .Include(a => a.Governorate)
                .Include(a => a.AttractionCategories)
                    .ThenInclude(ac => ac.Category)
                .ToListAsync();
            return View(attractions);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "إضافة معلم جديد";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttractionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _attractionService.CreateAttractionAsync(model);
                TempData["Success"] = "تم إضافة المعلم بنجاح";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "إضافة معلم جديد";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var attraction = await _context.Attractions
                .Include(a => a.AttractionCategories)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attraction == null)
                return NotFound();

            var model = new AttractionEditViewModel
            {
                Id = attraction.Id,
                Name = attraction.Name,
                Description = attraction.Description,
                Location = attraction.Location,
                GovernorateId = attraction.GovernorateId,
                SelectedCategoryIds = attraction.AttractionCategories.Select(ac => ac.CategoryId).ToList(),
                ExistingImageUrl = attraction.ImageUrl
            };

            ViewBag.Title = "تعديل المعلم";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AttractionEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _attractionService.UpdateAttractionAsync(model);
                TempData["Success"] = "تم تعديل المعلم بنجاح";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Title = "تعديل المعلم";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name");
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var attraction = await _context.Attractions
                .Include(a => a.Governorate)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attraction == null)
                return NotFound();

            ViewBag.Title = "حذف المعلم";
            return View(attraction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _attractionService.DeleteAttractionAsync(id);
            TempData["Success"] = "تم حذف المعلم بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}