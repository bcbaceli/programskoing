using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class WeddingCeremonyFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Wedding")]
        public int WeddingId { get; set; }

        [Required]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        [Required]
        [Display(Name = "Band")]
        public int BandId { get; set; }

        [Required]
        [Display(Name = "Florist")]
        public int FloristId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 100)]
        [Display(Name = "VAT %")]
        public decimal VAT { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public IEnumerable<SelectListItem> Weddings { get; set; } = [];
        public IEnumerable<SelectListItem> Restaurants { get; set; } = [];
        public IEnumerable<SelectListItem> Bands { get; set; } = [];
        public IEnumerable<SelectListItem> Florists { get; set; } = [];
    }
}
