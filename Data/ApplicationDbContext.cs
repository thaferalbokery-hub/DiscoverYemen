using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DiscoverYemen.Models;

namespace DiscoverYemen.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AttractionCategory> AttractionCategories { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingItem> BookingItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One-to-One: ApplicationUser -> UserProfile
            builder.Entity<UserProfile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(p => p.UserId);

            // Many-to-Many: Attraction <-> Category via AttractionCategory
            builder.Entity<AttractionCategory>()
                .HasKey(ac => new { ac.AttractionId, ac.CategoryId });

            builder.Entity<AttractionCategory>()
                .HasOne(ac => ac.Attraction)
                .WithMany(a => a.AttractionCategories)
                .HasForeignKey(ac => ac.AttractionId);

            builder.Entity<AttractionCategory>()
                .HasOne(ac => ac.Category)
                .WithMany(c => c.AttractionCategories)
                .HasForeignKey(ac => ac.CategoryId);

            // One-to-Many: Governorate -> Attractions
            builder.Entity<Attraction>()
                .HasOne(a => a.Governorate)
                .WithMany(g => g.Attractions)
                .HasForeignKey(a => a.GovernorateId);

            // One-to-Many: Governorate -> Hotels
            builder.Entity<Hotel>()
                .HasOne(h => h.Governorate)
                .WithMany(g => g.Hotels)
                .HasForeignKey(h => h.GovernorateId);

            // One-to-Many: Governorate -> Restaurants
            builder.Entity<Restaurant>()
                .HasOne(r => r.Governorate)
                .WithMany(g => g.Restaurants)
                .HasForeignKey(r => r.GovernorateId);

            // One-to-Many: Governorate -> Events
            builder.Entity<Event>()
                .HasOne(e => e.Governorate)
                .WithMany(g => g.Events)
                .HasForeignKey(e => e.GovernorateId);

            // One-to-Many: ApplicationUser -> Reviews
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            // One-to-Many: ApplicationUser -> Bookings
            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            // One-to-Many: Booking -> BookingItems
            builder.Entity<BookingItem>()
                .HasOne(bi => bi.Booking)
                .WithMany(b => b.BookingItems)
                .HasForeignKey(bi => bi.BookingId);

            // Favorite unique constraint
            builder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.AttractionId })
                .IsUnique();
        }
    }
}