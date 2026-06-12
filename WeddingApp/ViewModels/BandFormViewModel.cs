using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class BandFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 7)]
        [Display(Name = "Day of Week")]
        public int DayAtWeek { get; set; }

        [Required]
        [Range(1, 24)]
        [Display(Name = "Hours per Wedding")]
        public int HoursPerWedding { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Total Price")]
        public int TotalPrice { get; set; }

        [Required]
        [Display(Name = "Partner")]
        public int PartnerId { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; } = [];
    }
}
