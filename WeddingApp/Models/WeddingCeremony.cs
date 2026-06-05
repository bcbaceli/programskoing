namespace WeddingApp.Models
{
    public class WeddingCeremony
    {
        public int Id { get; set; }
        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; } = null!;
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public int BandId { get; set; }
        public Band Band { get; set; } = null!;

        public int FloristId { get; set; }
        public Florist Florist { get; set; } = null!;

        public decimal Price { get; set; }
        public decimal VAT { get; set; }
        public decimal TotalPrice { get; set; }

        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}