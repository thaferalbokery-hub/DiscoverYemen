using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverYemen.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "التعليق مطلوب")]
        [StringLength(1000, ErrorMessage = "التعليق يجب ألا يتجاوز 1000 حرف")]
        [Display(Name = "التعليق")]
        public string Comment { get; set; } = string.Empty;

        [Required(ErrorMessage = "التقييم مطلوب")]
        [Range(1, 5, ErrorMessage = "التقييم يجب أن يكون بين 1 و 5")]
        [Display(Name = "التقييم")]
        public int Rating { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int AttractionId { get; set; }

        [ForeignKey("AttractionId")]
        public Attraction Attraction { get; set; } = null!;
    }
}