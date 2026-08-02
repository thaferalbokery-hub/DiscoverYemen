using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverYemen.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int AttractionId { get; set; }

        [ForeignKey("AttractionId")]
        public Attraction Attraction { get; set; } = null!;

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}