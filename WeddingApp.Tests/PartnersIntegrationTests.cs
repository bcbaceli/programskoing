using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.Controllers;
using WeddingApp.Data;
using WeddingApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WeddingApp.Tests
{
    public class PartnersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public PartnersIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services => {
                    var toRemove = services.Where(d =>
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                        d.ServiceType == typeof(DbContext) ||
                        d.ServiceType == typeof(AppDbContext) ||
                        d.ServiceType.Name.Contains("DbContext")).ToList();

                    foreach (var d in toRemove) services.Remove(d);

                    var dbName = "IntTestBaza_" + Guid.NewGuid();

                    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(dbName));

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();

                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    context.Database.EnsureCreated();

                    context.PartnerCategories.Add(new PartnerCategory { Id = 1, Name = "Bend" });
                    context.Partners.Add(new Partner { Id = 1, Name = "Test", CategoryId = 1, Email = "t@t.com", CommissionPct = 10 });

                    context.SaveChanges();
                });
            });
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Partners_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/Partners");
            response.EnsureSuccessStatusCode();
        }
    }
}