using System.ComponentModel.DataAnnotations;

namespace WeddingApp.ViewModels
{
    public class PartnerCategoryFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
