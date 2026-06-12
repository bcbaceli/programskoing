using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class PlaylistFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Band")]
        public int BandId { get; set; }

        public IEnumerable<SelectListItem> Bands { get; set; } = [];
    }
}
