using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class FloristFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Arrangement Name")]
        public string ArrangementName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Item")]
        public int ItemId { get; set; }

        [Required]
        [Display(Name = "Partner")]
        public int PartnerId { get; set; }

        public IEnumerable<SelectListItem> Items { get; set; } = [];
        public IEnumerable<SelectListItem> Partners { get; set; } = [];
    }
}
