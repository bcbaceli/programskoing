using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Controllers;
using WeddingApp.Data;
using WeddingApp.Models;

namespace WeddingApp.Tests
{
    public class PartnersControllerTests
    {
        private AppDbContext KreirajBazu(string ime)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: ime)
                .Options;

            return new AppDbContext(options);
        }
        // test za dohvaćanje partnera
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfPartners()
        {
            var context = KreirajBazu("TestBaza_Index");
            var kategorija = new PartnerCategory { Id = 1, Name = "Bend" };
            context.PartnerCategories.Add(kategorija);
            context.Partners.AddRange
                (
                    new Partner { Id = 1, Name = "Partner 1", CategoryId = 1 },
                    new Partner { Id = 1, Name = "Partner 2", CategoryId = 1 }
                );
            await context.SaveChangesAsync();
            var controller = new PartnerController(context);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Partner>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        //test za dodavanje partnera
        [Fact]
        public async Task Create_ValidPartner_RedirectsToIndex()
        {

            var context = KreirajBazu("TestBaza_Create");
            context.PartnerCategories.Add(new PartnerCategory { Id = 1, Name = "Bend" });
            await context.SaveChangesAsync();
            var controller = new PartnerController(context);
            var noviPartner = new Partner { Name = "Test Band", CategoryId = 1, Email = "test@test.com", CommissionPct = 10 };


            var result = await controller.Create(noviPartner);


            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal(1, context.Partners.Count());
        }

        // test za brisanje partnera
        [Fact]
        public async Task Delete_NonExistentId_ReturnsNotFound()
        {

            var context = KreirajBazu("TestBaza_Delete");
            var controller = new PartnerController(context);


            var result = await controller.Delete(999);


            Assert.IsType<NotFoundResult>(result);
        }


    }
}