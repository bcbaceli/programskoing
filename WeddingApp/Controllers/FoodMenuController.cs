using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class FoodMenuController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public FoodMenuController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.FoodMenus.Include(x => x.Restaurant).AsNoTracking();
            query = (sortBy, sortDir) switch
            {
                ("name",       "desc") => query.OrderByDescending(x => x.Name),
                ("restaurant", "asc")  => query.OrderBy(x => x.Restaurant.Name),
                ("restaurant", "desc") => query.OrderByDescending(x => x.Restaurant.Name),
                ("price",      "asc")  => query.OrderBy(x => x.PricePerPerson),
                ("price",      "desc") => query.OrderByDescending(x => x.PricePerPerson),
                _ => query.OrderBy(x => x.Name)
            };
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            return View(new IndexViewModel<FoodMenu> { Items = items, CurrentPage = page, TotalPages = (int)Math.Ceiling(total / (double)PageSize), SortBy = sortBy, SortDir = sortDir });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.FoodMenus.Include(x => x.Restaurant).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new FoodMenuFormViewModel { Restaurants = await GetRestaurantsAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(FoodMenuFormViewModel vm)
        {
            if (!ModelState.IsValid) { vm.Restaurants = await GetRestaurantsAsync(); return View(vm); }
            _context.FoodMenus.Add(new FoodMenu { Name = vm.Name, PricePerPerson = vm.PricePerPerson, RestaurantId = vm.RestaurantId });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Food menu '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.FoodMenus.FindAsync(id);
            if (item == null) return NotFound();
            return View(new FoodMenuFormViewModel { Id = item.Id, Name = item.Name, PricePerPerson = item.PricePerPerson, RestaurantId = item.RestaurantId, Restaurants = await GetRestaurantsAsync(item.RestaurantId) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FoodMenuFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) { vm.Restaurants = await GetRestaurantsAsync(vm.RestaurantId); return View(vm); }
            var item = await _context.FoodMenus.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = vm.Name; item.PricePerPerson = vm.PricePerPerson; item.RestaurantId = vm.RestaurantId;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Food menu '{item.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.FoodMenus.Include(x => x.Restaurant).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.FoodMenus.FindAsync(id);
            if (item != null) { _context.FoodMenus.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Food menu '{item.Name}' deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetRestaurantsAsync(int? selectedId = null) =>
            await _context.Restaurants.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
