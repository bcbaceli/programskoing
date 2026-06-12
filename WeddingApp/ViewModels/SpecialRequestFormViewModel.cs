using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class SpecialRequestFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        public IEnumerable<SelectListItem> Restaurants { get; set; } = [];
    }
}
