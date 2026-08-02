using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverYemen.Models
{
    public class BookingItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; } = null!;

        [Required(ErrorMessage = "نوع العنصر مطلوب")]
        [StringLength(50)]
        [Display(Name = "نوع العنصر")]
        public string ItemType { get; set; } = string.Empty; // Hotel, Attraction, Event

        [Required]
        [Display(Name = "معرف العنصر")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "اسم العنصر مطلوب")]
        [StringLength(200)]
        [Display(Name = "اسم العنصر")]
        public string ItemName { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "الكمية يجب أن تكون بين 1 و 100")]
        [Display(Name = "الكمية")]
        public int Quantity { get; set; } = 1;

        [Display(Name = "السعر")]
        public decimal Price { get; set; }
    }
}