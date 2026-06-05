namespace WeddingApp.Models
{
    public class FoodMenu
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerPerson { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;
    }
}