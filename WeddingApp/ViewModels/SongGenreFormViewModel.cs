using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class SongGenreFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
