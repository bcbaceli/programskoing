using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class RestaurantFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Number of Guests")]
        public int NumberOfGuests { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Display(Name = "Partner")]
        public int PartnerId { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; } = [];
    }
}
