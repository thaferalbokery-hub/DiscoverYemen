using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscoverYemen.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب ألا يتجاوز 100 حرف")]
        [Display(Name = "اسم التصنيف")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        public ICollection<AttractionCategory> AttractionCategories { get; set; } = new List<AttractionCategory>();
    }
}