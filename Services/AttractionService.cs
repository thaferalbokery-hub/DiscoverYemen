using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;
using DiscoverYemen.Models;
using DiscoverYemen.ViewModels;

namespace DiscoverYemen.Services
{
    public class AttractionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AttractionService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<AttractionListViewModel>> GetAttractionListAsync(int? governorateId, int? categoryId, string? searchTerm)
        {
            var query = _context.Attractions
                .Include(a => a.Governorate)
                .Include(a => a.AttractionCategories)
                    .ThenInclude(ac => ac.Category)
                .AsQueryable();

            if (governorateId.HasValue)
            {
                query = query.Where(a => a.GovernorateId == governorateId.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.AttractionCategories.Any(ac => ac.CategoryId == categoryId.Value));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Name.Contains(searchTerm) || a.Description.Contains(searchTerm));
            }

            var attractions = await query
                .Select(a => new AttractionListViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    ImageUrl = a.ImageUrl,
                    GovernorateName = a.Governorate.Name,
                    Categories = a.AttractionCategories.Select(ac => ac.Category.Name).ToList()
                })
                .ToListAsync();

            return attractions;
        }

        public async Task<Attraction?> GetAttractionDetailsAsync(int id)
        {
            return await _context.Attractions
                .Include(a => a.Governorate)
                .Include(a => a.AttractionCategories)
                    .ThenInclude(ac => ac.Category)
                .Include(a => a.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Attraction> CreateAttractionAsync(AttractionCreateViewModel model)
        {
            var attraction = new Attraction
            {
                Name = model.Name,
                Description = model.Description,
                Location = model.Location,
                GovernorateId = model.GovernorateId
            };

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                attraction.ImageUrl = await SaveImageAsync(model.ImageFile);
            }

            _context.Attractions.Add(attraction);
            await _context.SaveChangesAsync();

            // Add categories
            if (model.SelectedCategoryIds.Any())
            {
                foreach (var catId in model.SelectedCategoryIds)
                {
                    _context.AttractionCategories.Add(new AttractionCategory
                    {
                        AttractionId = attraction.Id,
                        CategoryId = catId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return attraction;
        }

        public async Task UpdateAttractionAsync(AttractionEditViewModel model)
        {
            var attraction = await _context.Attractions
                .Include(a => a.AttractionCategories)
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (attraction == null) return;

            attraction.Name = model.Name;
            attraction.Description = model.Description;
            attraction.Location = model.Location;
            attraction.GovernorateId = model.GovernorateId;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(attraction.ImageUrl))
                {
                    DeleteImage(attraction.ImageUrl);
                }
                attraction.ImageUrl = await SaveImageAsync(model.ImageFile);
            }

            // Update categories
            _context.AttractionCategories.RemoveRange(attraction.AttractionCategories);
            foreach (var catId in model.SelectedCategoryIds)
            {
                _context.AttractionCategories.Add(new AttractionCategory
                {
                    AttractionId = attraction.Id,
                    CategoryId = catId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAttractionAsync(int id)
        {
            var attraction = await _context.Attractions.FindAsync(id);
            if (attraction == null) return;

            // Delete associated image
            if (!string.IsNullOrEmpty(attraction.ImageUrl))
            {
                DeleteImage(attraction.ImageUrl);
            }

            _context.Attractions.Remove(attraction);
            await _context.SaveChangesAsync();
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/" + uniqueFileName;
        }

        private void DeleteImage(string imageUrl)
        {
            var filePath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}