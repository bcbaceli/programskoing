namespace WeddingApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Florist> Florists { get; set; } = new List<Florist>();
        public ICollection<Pastry> Pastries { get; set; } = new List<Pastry>();
    }
}