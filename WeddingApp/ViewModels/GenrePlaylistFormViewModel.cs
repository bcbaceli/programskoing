using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class GenrePlaylistFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Required]
        [Display(Name = "Playlist")]
        public int PlaylistId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = [];
        public IEnumerable<SelectListItem> Playlists { get; set; } = [];
    }
}
