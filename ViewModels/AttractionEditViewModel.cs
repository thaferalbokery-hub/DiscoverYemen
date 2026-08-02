using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DiscoverYemen.ViewModels
{
    public class AttractionEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المعلم مطلوب")]
        [StringLength(200, ErrorMessage = "الاسم يجب ألا يتجاوز 200 حرف")]
        [Display(Name = "اسم المعلم")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "الوصف مطلوب")]
        [StringLength(2000, ErrorMessage = "الوصف يجب ألا يتجاوز 2000 حرف")]
        [Display(Name = "الوصف")]
        public string Description { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "الموقع")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "المحافظة مطلوبة")]
        [Display(Name = "المحافظة")]
        public int GovernorateId { get; set; }

        [Display(Name = "التصنيفات")]
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();

        [Display(Name = "صورة جديدة")]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImageUrl { get; set; }
    }
}