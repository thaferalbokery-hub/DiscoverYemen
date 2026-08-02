using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverYemen.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الفعالية مطلوب")]
        [StringLength(200, ErrorMessage = "الاسم يجب ألا يتجاوز 200 حرف")]
        [Display(Name = "اسم الفعالية")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "تاريخ البدء مطلوب")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ البدء")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الانتهاء")]
        public DateTime? EndDate { get; set; }

        [StringLength(300)]
        [Display(Name = "الموقع")]
        public string? Location { get; set; }

        [StringLength(50)]
        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "الصورة")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "المحافظة مطلوبة")]
        [Display(Name = "المحافظة")]
        public int GovernorateId { get; set; }

        [ForeignKey("GovernorateId")]
        public Governorate Governorate { get; set; } = null!;
    }
}