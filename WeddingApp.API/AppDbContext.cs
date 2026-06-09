using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<WeddingApp.API.Models.PartnerDto> PartnerDto { get; set; } = default!;
}
