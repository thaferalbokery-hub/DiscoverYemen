using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverYemen.Models
{
    public class AttractionCategory
    {
        public int AttractionId { get; set; }

        [ForeignKey("AttractionId")]
        public Attraction Attraction { get; set; } = null!;

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;
    }
}