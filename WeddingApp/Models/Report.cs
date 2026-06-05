namespace WeddingApp.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;

        public int WeddingId { get; set; }
        public WeddingCeremony Wedding { get; set; } = null!;
    }
}