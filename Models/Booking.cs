using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverYemen.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الحجز")]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [StringLength(50)]
        [Display(Name = "الحالة")]
        public string Status { get; set; } = "مؤكد";

        [Display(Name = "الإجمالي")]
        public decimal TotalAmount { get; set; }

        public ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();
    }
}