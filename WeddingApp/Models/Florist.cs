namespace WeddingApp.Models
{
    public class Florist
    {
        public int Id { get; set; }
        public string ArrangementName { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public int PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;
        public ICollection<WeddingCeremony> WeddingCeremonies { get; set; } = new List<WeddingCeremony>();
    }
}