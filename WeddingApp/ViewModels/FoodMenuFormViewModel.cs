using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class FoodMenuFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Price per Person")]
        public decimal PricePerPerson { get; set; }

        [Required]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        public IEnumerable<SelectListItem> Restaurants { get; set; } = [];
    }
}
