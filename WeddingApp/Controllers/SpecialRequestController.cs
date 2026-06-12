using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SpecialRequestController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public SpecialRequestController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.SpecialRequests.Include(x => x.Restaurant).AsNoTracking();

            query = (sortBy, sortDir) switch
            {
                ("name",       "desc") => query.OrderByDescending(x => x.Name),
                ("restaurant", "asc")  => query.OrderBy(x => x.Restaurant.Name),
                ("restaurant", "desc") => query.OrderByDescending(x => x.Restaurant.Name),
                _ => query.OrderBy(x => x.Name)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            return View(new IndexViewModel<SpecialRequest>
            {
                Items = items, CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)PageSize),
                SortBy = sortBy, SortDir = sortDir
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.SpecialRequests.Include(x => x.Restaurant).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new SpecialRequestFormViewModel { Restaurants = await GetRestaurantsAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(SpecialRequestFormViewModel vm)
        {
            if (!ModelState.IsValid) { vm.Restaurants = await GetRestaurantsAsync(); return View(vm); }

            _context.SpecialRequests.Add(new SpecialRequest { Name = vm.Name, RestaurantId = vm.RestaurantId });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Special request '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.SpecialRequests.FindAsync(id);
            if (item == null) return NotFound();
            return View(new SpecialRequestFormViewModel
            {
                Id = item.Id, Name = item.Name, RestaurantId = item.RestaurantId,
                Restaurants = await GetRestaurantsAsync(item.RestaurantId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SpecialRequestFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) { vm.Restaurants = await GetRestaurantsAsync(vm.RestaurantId); return View(vm); }

            var item = await _context.SpecialRequests.FindAsync(id);
            if (item == null) return NotFound();

            item.Name = vm.Name; item.RestaurantId = vm.RestaurantId;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Special request '{item.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.SpecialRequests.Include(x => x.Restaurant).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.SpecialRequests.FindAsync(id);
            if (item != null) { _context.SpecialRequests.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Special request '{item.Name}' deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetRestaurantsAsync(int? selectedId = null) =>
            await _context.Restaurants.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
