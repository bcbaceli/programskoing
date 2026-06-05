namespace WeddingApp.Models
{
    public class SpecialRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;
    }
}