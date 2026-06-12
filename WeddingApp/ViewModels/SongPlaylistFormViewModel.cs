using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class SongPlaylistFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Song")]
        public int SongId { get; set; }

        [Required]
        [Display(Name = "Playlist")]
        public int PlaylistId { get; set; }

        public IEnumerable<SelectListItem> Songs { get; set; } = [];
        public IEnumerable<SelectListItem> Playlists { get; set; } = [];
    }
}
