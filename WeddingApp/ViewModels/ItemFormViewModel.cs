using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class ItemFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}
