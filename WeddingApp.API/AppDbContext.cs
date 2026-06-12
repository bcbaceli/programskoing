using Microsoft.EntityFrameworkCore;
using WeddingApp.API.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PartnerCategory> PartnerCategories => Set<PartnerCategory>();
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<WeddingTemplate> WeddingTemplates => Set<WeddingTemplate>();
}
