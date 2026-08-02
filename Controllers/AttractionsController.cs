using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;
using DiscoverYemen.Services;

namespace DiscoverYemen.Controllers
{
    public class AttractionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttractionService _attractionService;

        public AttractionsController(ApplicationDbContext context, AttractionService attractionService)
        {
            _context = context;
            _attractionService = attractionService;
        }

        public async Task<IActionResult> Index(int? governorateId, int? categoryId, string? searchTerm)
        {
            ViewBag.Title = "المعالم السياحية";
            ViewBag.Governorates = new SelectList(await _context.Governorates.ToListAsync(), "Id", "Name", governorateId);
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", categoryId);
            ViewData["CurrentSearch"] = searchTerm;
            ViewData["CurrentGovernorate"] = governorateId;
            ViewData["CurrentCategory"] = categoryId;

            var attractions = await _attractionService.GetAttractionListAsync(governorateId, categoryId, searchTerm);
            return View(attractions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var attraction = await _attractionService.GetAttractionDetailsAsync(id);
            if (attraction == null)
                return NotFound();

            ViewBag.Title = attraction.Name;
            return View(attraction);
        }
    }
}