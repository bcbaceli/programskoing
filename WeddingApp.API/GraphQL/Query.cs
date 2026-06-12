using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using WeddingApp.API.Models;

namespace WeddingApp.API.GraphQL
{
    public class Query
    {
        // Partners
        [UseFiltering]
        [UseSorting]
        public IQueryable<Partner> GetPartners([Service] IDbContextFactory<AppDbContext> factory)
        {
            var context = factory.CreateDbContext();
            return context.Partners.Include(p => p.Category);
        }

        public async Task<Partner?> GetPartner(int id, [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            return await context.Partners
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Partner Categories
        [UseFiltering]
        [UseSorting]
        public IQueryable<PartnerCategory> GetPartnerCategories([Service] IDbContextFactory<AppDbContext> factory)
        {
            var context = factory.CreateDbContext();
            return context.PartnerCategories;
        }

        public async Task<PartnerCategory?> GetPartnerCategory(int id, [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            return await context.PartnerCategories
                .Include(c => c.Partners)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Wedding Templates
        [UseFiltering]
        [UseSorting]
        public IQueryable<WeddingTemplate> GetWeddingTemplates([Service] IDbContextFactory<AppDbContext> factory)
        {
            var context = factory.CreateDbContext();
            return context.WeddingTemplates;
        }

        public async Task<WeddingTemplate?> GetWeddingTemplate(int id, [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            return await context.WeddingTemplates.FindAsync(id);
        }
    }
}
