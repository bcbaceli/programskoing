namespace WeddingApp.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }

        public ICollection<FoodMenu> FoodMenus { get; set; } = new List<FoodMenu>();
        public ICollection<SpecialRequest> SpecialRequests { get; set; } = new List<SpecialRequest>();
        public int PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;

        public ICollection<WeddingCeremony> WeddingCeremonies { get; set; } = new List<WeddingCeremony>();
    }
}