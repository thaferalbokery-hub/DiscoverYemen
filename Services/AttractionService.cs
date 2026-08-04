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

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/png", "image/webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

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

        /// <summary>
        /// Validates an uploaded image file. Returns null if valid, or an error message string if invalid.
        /// </summary>
        public string? ValidateImageFile(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return "الملف فارغ أو غير موجود";
            }

            if (file.Length > MaxFileSize)
            {
                return "حجم الملف يتجاوز الحد المسموح (5 ميجابايت)";
            }

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
            {
                return "نوع الملف غير مسموح. الأنواع المسموحة: JPG, JPEG, PNG, WEBP";
            }

            var contentType = file.ContentType?.ToLowerInvariant();
            if (string.IsNullOrEmpty(contentType) || !AllowedMimeTypes.Contains(contentType))
            {
                return "نوع المحتوى غير مسموح. الأنواع المسموحة: image/jpeg, image/png, image/webp";
            }

            return null;
        }

        public async Task<(Attraction? attraction, string? error)> CreateAttractionAsync(AttractionCreateViewModel model)
        {
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var validationError = ValidateImageFile(model.ImageFile);
                if (validationError != null)
                {
                    return (null, validationError);
                }
            }

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

            return (attraction, null);
        }

        public async Task<string?> UpdateAttractionAsync(AttractionEditViewModel model)
        {
            var attraction = await _context.Attractions
                .Include(a => a.AttractionCategories)
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (attraction == null) return "المعلم غير موجود";

            // Validate new image before making any changes
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var validationError = ValidateImageFile(model.ImageFile);
                if (validationError != null)
                {
                    return validationError;
                }
            }

            attraction.Name = model.Name;
            attraction.Description = model.Description;
            attraction.Location = model.Location;
            attraction.GovernorateId = model.GovernorateId;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Save new image first, then delete old one
                var newImageUrl = await SaveImageAsync(model.ImageFile);
                var oldImageUrl = attraction.ImageUrl;

                attraction.ImageUrl = newImageUrl;

                // Delete old image after successful save
                if (!string.IsNullOrEmpty(oldImageUrl))
                {
                    DeleteImage(oldImageUrl);
                }
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
            return null;
        }

        public async Task DeleteAttractionAsync(int id)
        {
            var attraction = await _context.Attractions.FindAsync(id);
            if (attraction == null) return;

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

            // Generate safe unique filename using Guid — never trust the original filename
            var safeExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? ".jpg";
            var uniqueFileName = Guid.NewGuid().ToString() + safeExtension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/" + uniqueFileName;
        }

        private void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            // Prevent path traversal by only allowing files within the uploads directory
            var fileName = Path.GetFileName(imageUrl);
            if (string.IsNullOrEmpty(fileName)) return;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Verify the resolved path is still within uploads directory
            var fullPath = Path.GetFullPath(filePath);
            var fullUploadsPath = Path.GetFullPath(uploadsFolder);
            if (!fullPath.StartsWith(fullUploadsPath, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                }
                catch (IOException)
                {
                    // File may be in use or already deleted; do not throw
                }
            }
        }
    }
}