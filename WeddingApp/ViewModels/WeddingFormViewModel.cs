using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class WeddingFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateOnly Date { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [Phone]
        [StringLength(50)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Template")]
        public int TemplateId { get; set; }

        public IEnumerable<SelectListItem> Templates { get; set; } = [];
    }
}
