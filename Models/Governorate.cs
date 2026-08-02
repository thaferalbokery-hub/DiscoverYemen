using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscoverYemen.Models
{
    public class Governorate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المحافظة مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب ألا يتجاوز 100 حرف")]
        [Display(Name = "اسم المحافظة")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "الوصف يجب ألا يتجاوز 1000 حرف")]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "الصورة")]
        public string? ImageUrl { get; set; }

        public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}