using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverYemen.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الفندق مطلوب")]
        [StringLength(200, ErrorMessage = "الاسم يجب ألا يتجاوز 200 حرف")]
        [Display(Name = "اسم الفندق")]
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

        [Range(1, 5, ErrorMessage = "التقييم يجب أن يكون بين 1 و 5")]
        [Display(Name = "عدد النجوم")]
        public int? Stars { get; set; }

        [Display(Name = "الصورة")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "المحافظة مطلوبة")]
        [Display(Name = "المحافظة")]
        public int GovernorateId { get; set; }

        [ForeignKey("GovernorateId")]
        public Governorate Governorate { get; set; } = null!;
    }
}