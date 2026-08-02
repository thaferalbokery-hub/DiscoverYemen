using System.ComponentModel.DataAnnotations;

namespace DiscoverYemen.ViewModels
{
    public class BookingCreateViewModel
    {
        [Required(ErrorMessage = "نوع العنصر مطلوب")]
        [Display(Name = "نوع العنصر")]
        public string ItemType { get; set; } = string.Empty;

        [Required]
        [Display(Name = "العنصر")]
        public int ItemId { get; set; }

        [Required]
        [Display(Name = "اسم العنصر")]
        public string ItemName { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "الكمية يجب أن تكون بين 1 و 100")]
        [Display(Name = "الكمية")]
        public int Quantity { get; set; } = 1;

        [StringLength(500)]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }
    }
}