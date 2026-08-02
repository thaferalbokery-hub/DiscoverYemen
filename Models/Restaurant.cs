using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverYemen.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المطعم مطلوب")]
        [StringLength(200, ErrorMessage = "الاسم يجب ألا يتجاوز 200 حرف")]
        [Display(Name = "اسم المطعم")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [StringLength(300)]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [Phone]
        [Display(Name = "رقم الهاتف")]
        public string? Phone { get; set; }

        [StringLength(100)]
        [Display(Name = "نوع المطبخ")]
        public string? CuisineType { get; set; }

        [Display(Name = "الصورة")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "المحافظة مطلوبة")]
        [Display(Name = "المحافظة")]
        public int GovernorateId { get; set; }

        [ForeignKey("GovernorateId")]
        public Governorate Governorate { get; set; } = null!;
    }
}