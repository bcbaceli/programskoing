namespace WeddingApp.Models
{
    public class Wedding
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int TemplateId { get; set; }

        public WeddingTemplate Template { get; set; } = null!;
    }
}