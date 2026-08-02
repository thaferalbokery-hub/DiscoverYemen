using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;
using DiscoverYemen.ViewModels;

namespace DiscoverYemen.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportViewModel> GetReportAsync()
        {
            var report = new ReportViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalGovernorates = await _context.Governorates.CountAsync(),
                TotalAttractions = await _context.Attractions.CountAsync(),
                TotalHotels = await _context.Hotels.CountAsync(),
                TotalRestaurants = await _context.Restaurants.CountAsync(),
                TotalEvents = await _context.Events.CountAsync(),
                TotalReviews = await _context.Reviews.CountAsync(),
                TotalBookings = await _context.Bookings.CountAsync(),
                AttractionsByGovernorate = await _context.Governorates
                    .Select(g => new GovernorateAttractionCount
                    {
                        GovernorateName = g.Name,
                        Count = g.Attractions.Count()
                    })
                    .Where(x => x.Count > 0)
                    .ToListAsync()
            };

            return report;
        }
    }
}