using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class PartnerFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [Phone]
        [StringLength(50)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        [Required]
        [Range(0, 100)]
        [Display(Name = "Commission %")]
        public decimal CommissionPct { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = [];
    }
}
