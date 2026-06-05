namespace WeddingApp.Models
{
    public class WeddingTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<Wedding> Weddings { get; set; } = new List<Wedding>();
    }
}