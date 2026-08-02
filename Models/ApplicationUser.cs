using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DiscoverYemen.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public UserProfile? Profile { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}