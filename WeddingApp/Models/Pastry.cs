namespace WeddingApp.Models
{
    public class Pastry
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public int PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;
    }
}