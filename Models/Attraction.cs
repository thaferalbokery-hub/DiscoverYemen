using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverYemen.Models
{
    public class Attraction
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المعلم مطلوب")]
        [StringLength(200, ErrorMessage = "الاسم يجب ألا يتجاوز 200 حرف")]
        [Display(Name = "اسم المعلم")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "الوصف مطلوب")]
        [StringLength(2000, ErrorMessage = "الوصف يجب ألا يتجاوز 2000 حرف")]
        [Display(Name = "الوصف")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "الموقع")]
        [StringLength(300)]
        public string? Location { get; set; }

        [Display(Name = "الصورة")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "المحافظة مطلوبة")]
        [Display(Name = "المحافظة")]
        public int GovernorateId { get; set; }

        [ForeignKey("GovernorateId")]
        public Governorate Governorate { get; set; } = null!;

        public ICollection<AttractionCategory> AttractionCategories { get; set; } = new List<AttractionCategory>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}