namespace WeddingApp.Models
{
    public class Band
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DayAtWeek { get; set; }
        public int HoursPerWedding { get; set; }
        public int TotalPrice { get; set; }
        public int PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;

        public ICollection<WeddingCeremony> WeddingCeremonies { get; set; } = new List<WeddingCeremony>();
        public ICollection<Band> Bands { get; set; } = new List<Band>();
    }
}