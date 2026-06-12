using HotChocolate;
using Microsoft.EntityFrameworkCore;
using WeddingApp.API.Models;

namespace WeddingApp.API.GraphQL
{
    public class Mutation
    {
        // --- Partner ---

        public async Task<Partner> AddPartner(
            string name, string? address, string? phone, string? email,
            int categoryId, decimal commissionPct,
            [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            var partner = new Partner
            {
                Name = name,
                Address = address,
                Phone = phone,
                Email = email,
                CategoryId = categoryId,
                CommissionPct = commissionPct
            };
            context.Partners.Add(partner);
            await context.SaveChangesAsync();
            return partner;
        }

        public async Task<Partner?> UpdatePartner(
            int id, string name, string? address, string? phone, string? email,
            int categoryId, decimal commissionPct,
            [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            var partner = await context.Partners.FindAsync(id);
            if (partner == null) return null;

            partner.Name = name;
            partner.Address = address;
            partner.Phone = phone;
            partner.Email = email;
            partner.CategoryId = categoryId;
            partner.CommissionPct = commissionPct;

            await context.SaveChangesAsync();
            return partner;
        }

        public async Task<bool> DeletePartner(int id, [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            var partner = await context.Partners.FindAsync(id);
            if (partner == null) return false;

            context.Partners.Remove(partner);
            await context.SaveChangesAsync();
            return true;
        }

        // --- PartnerCategory ---

        public async Task<PartnerCategory> AddPartnerCategory(
            string name, string? description,
            [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            var category = new PartnerCategory { Name = name, Description = description };
            context.PartnerCategories.Add(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task<PartnerCategory?> UpdatePartnerCategory(
            int id, string name, string? description,
            [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            var category = await context.PartnerCategories.FindAsync(id);
            if (category == null) return null;

            category.Name = name;
            category.Description = description;

            await context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeletePartnerCategory(int id, [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            var category = await context.PartnerCategories.FindAsync(id);
            if (category == null) return false;

            context.PartnerCategories.Remove(category);
            await context.SaveChangesAsync();
            return true;
        }

        // --- WeddingTemplate ---

        public async Task<WeddingTemplate> AddWeddingTemplate(
            string name, string? description,
            [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            var template = new WeddingTemplate { Name = name, Description = description };
            context.WeddingTemplates.Add(template);
            await context.SaveChangesAsync();
            return template;
        }

        public async Task<WeddingTemplate?> UpdateWeddingTemplate(
            int id, string name, string? description,
            [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            var template = await context.WeddingTemplates.FindAsync(id);
            if (template == null) return null;

            template.Name = name;
            template.Description = description;

            await context.SaveChangesAsync();
            return template;
        }

        public async Task<bool> DeleteWeddingTemplate(int id, [Service] IDbContextFactory<AppDbContext> factory)
        {
            using var context = await factory.CreateDbContextAsync();
            var template = await context.WeddingTemplates.FindAsync(id);
            if (template == null) return false;

            context.WeddingTemplates.Remove(template);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
