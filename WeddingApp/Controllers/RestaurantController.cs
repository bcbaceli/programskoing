using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class RestaurantController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public RestaurantController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.Restaurants.Include(x => x.Partner).AsNoTracking();

            query = (sortBy, sortDir) switch
            {
                ("name",    "desc") => query.OrderByDescending(x => x.Name),
                ("partner", "asc")  => query.OrderBy(x => x.Partner.Name),
                ("partner", "desc") => query.OrderByDescending(x => x.Partner.Name),
                ("guests",  "asc")  => query.OrderBy(x => x.NumberOfGuests),
                ("guests",  "desc") => query.OrderByDescending(x => x.NumberOfGuests),
                ("price",   "asc")  => query.OrderBy(x => x.TotalPrice),
                ("price",   "desc") => query.OrderByDescending(x => x.TotalPrice),
                _ => query.OrderBy(x => x.Name)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            return View(new IndexViewModel<Restaurant>
            {
                Items = items, CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)PageSize),
                SortBy = sortBy, SortDir = sortDir
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Restaurants.Include(x => x.Partner).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new RestaurantFormViewModel { Partners = await GetPartnersAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantFormViewModel vm)
        {
            if (!ModelState.IsValid) { vm.Partners = await GetPartnersAsync(); return View(vm); }

            _context.Restaurants.Add(new Restaurant
            {
                Name = vm.Name, NumberOfGuests = vm.NumberOfGuests,
                TotalPrice = vm.TotalPrice, PartnerId = vm.PartnerId
            });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Restaurant '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Restaurants.FindAsync(id);
            if (item == null) return NotFound();
            return View(new RestaurantFormViewModel
            {
                Id = item.Id, Name = item.Name, NumberOfGuests = item.NumberOfGuests,
                TotalPrice = item.TotalPrice, PartnerId = item.PartnerId,
                Partners = await GetPartnersAsync(item.PartnerId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RestaurantFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) { vm.Partners = await GetPartnersAsync(vm.PartnerId); return View(vm); }

            var item = await _context.Restaurants.FindAsync(id);
            if (item == null) return NotFound();

            item.Name = vm.Name; item.NumberOfGuests = vm.NumberOfGuests;
            item.TotalPrice = vm.TotalPrice; item.PartnerId = vm.PartnerId;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Restaurant '{item.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Restaurants.Include(x => x.Partner).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Restaurants.FindAsync(id);
            if (item != null) { _context.Restaurants.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Restaurant '{item.Name}' deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetPartnersAsync(int? selectedId = null) =>
            await _context.Partners.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
