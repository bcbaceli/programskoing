using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class ReportFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Partner")]
        public int PartnerId { get; set; }

        [Required]
        [Display(Name = "Wedding Ceremony")]
        public int WeddingId { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; } = [];
        public IEnumerable<SelectListItem> WeddingCeremonies { get; set; } = [];
    }
}
