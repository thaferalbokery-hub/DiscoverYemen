using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Data;
using DiscoverYemen.Models;
using DiscoverYemen.ViewModels;

namespace DiscoverYemen.Services
{
    public class BookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(string userId, BookingCreateViewModel model)
        {
            var booking = new Booking
            {
                UserId = userId,
                BookingDate = DateTime.UtcNow,
                Notes = model.Notes,
                Status = "مؤكد",
                TotalAmount = 0
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var bookingItem = new BookingItem
            {
                BookingId = booking.Id,
                ItemType = model.ItemType,
                ItemId = model.ItemId,
                ItemName = model.ItemName,
                Quantity = model.Quantity,
                Price = 0
            };

            _context.BookingItems.Add(bookingItem);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<List<Booking>> GetUserBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.BookingItems)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.BookingItems)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }
    }
}