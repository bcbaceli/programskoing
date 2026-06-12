namespace WeddingApp.Models
{
    public class Partner
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public decimal CommissionPct { get; set; }

        public virtual PartnerCategory? Category { get; set; }
        
    }
}