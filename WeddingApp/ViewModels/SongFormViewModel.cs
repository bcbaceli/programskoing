using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class SongFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = [];
    }
}
