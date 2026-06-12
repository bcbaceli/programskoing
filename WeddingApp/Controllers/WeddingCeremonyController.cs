using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class WeddingCeremonyController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public WeddingCeremonyController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "wedding", string sortDir = "asc", int page = 1)
        {
            var query = _context.WeddingCeremonies
                .Include(x => x.Wedding).Include(x => x.Restaurant)
                .Include(x => x.Band).Include(x => x.Florist)
                .AsNoTracking();

            query = (sortBy, sortDir) switch
            {
                ("wedding",    "desc") => query.OrderByDescending(x => x.Wedding.Date),
                ("restaurant", "asc")  => query.OrderBy(x => x.Restaurant.Name),
                ("restaurant", "desc") => query.OrderByDescending(x => x.Restaurant.Name),
                ("price",      "asc")  => query.OrderBy(x => x.TotalPrice),
                ("price",      "desc") => query.OrderByDescending(x => x.TotalPrice),
                _ => query.OrderBy(x => x.Wedding.Date)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            return View(new IndexViewModel<WeddingCeremony>
            {
                Items = items, CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)PageSize),
                SortBy = sortBy, SortDir = sortDir
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.WeddingCeremonies
                .Include(x => x.Wedding).Include(x => x.Restaurant)
                .Include(x => x.Band).Include(x => x.Florist)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() => View(new WeddingCeremonyFormViewModel
        {
            Weddings = await GetWeddingsAsync(), Restaurants = await GetRestaurantsAsync(),
            Bands = await GetBandsAsync(), Florists = await GetFloristsAsync()
        });

        [HttpPost]
        public async Task<IActionResult> Create(WeddingCeremonyFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Weddings = await GetWeddingsAsync(); vm.Restaurants = await GetRestaurantsAsync();
                vm.Bands = await GetBandsAsync(); vm.Florists = await GetFloristsAsync();
                return View(vm);
            }
            _context.WeddingCeremonies.Add(new WeddingCeremony
            {
                WeddingId = vm.WeddingId, RestaurantId = vm.RestaurantId,
                BandId = vm.BandId, FloristId = vm.FloristId,
                Price = vm.Price, VAT = vm.VAT, TotalPrice = vm.TotalPrice
            });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Wedding ceremony created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.WeddingCeremonies.FindAsync(id);
            if (item == null) return NotFound();
            return View(new WeddingCeremonyFormViewModel
            {
                Id = item.Id, WeddingId = item.WeddingId, RestaurantId = item.RestaurantId,
                BandId = item.BandId, FloristId = item.FloristId,
                Price = item.Price, VAT = item.VAT, TotalPrice = item.TotalPrice,
                Weddings = await GetWeddingsAsync(item.WeddingId), Restaurants = await GetRestaurantsAsync(item.RestaurantId),
                Bands = await GetBandsAsync(item.BandId), Florists = await GetFloristsAsync(item.FloristId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, WeddingCeremonyFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                vm.Weddings = await GetWeddingsAsync(vm.WeddingId); vm.Restaurants = await GetRestaurantsAsync(vm.RestaurantId);
                vm.Bands = await GetBandsAsync(vm.BandId); vm.Florists = await GetFloristsAsync(vm.FloristId);
                return View(vm);
            }
            var item = await _context.WeddingCeremonies.FindAsync(id);
            if (item == null) return NotFound();

            item.WeddingId = vm.WeddingId; item.RestaurantId = vm.RestaurantId;
            item.BandId = vm.BandId; item.FloristId = vm.FloristId;
            item.Price = vm.Price; item.VAT = vm.VAT; item.TotalPrice = vm.TotalPrice;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Wedding ceremony updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.WeddingCeremonies
                .Include(x => x.Wedding).Include(x => x.Restaurant)
                .Include(x => x.Band).Include(x => x.Florist)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.WeddingCeremonies.FindAsync(id);
            if (item != null) { _context.WeddingCeremonies.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = "Wedding ceremony deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetWeddingsAsync(int? selectedId = null) =>
            await _context.Weddings.AsNoTracking().OrderByDescending(x => x.Date)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Date.ToString("d") + (x.Address != null ? " — " + x.Address : ""), Selected = x.Id == selectedId })
                .ToListAsync();

        private async Task<IEnumerable<SelectListItem>> GetRestaurantsAsync(int? selectedId = null) =>
            await _context.Restaurants.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();

        private async Task<IEnumerable<SelectListItem>> GetBandsAsync(int? selectedId = null) =>
            await _context.Bands.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();

        private async Task<IEnumerable<SelectListItem>> GetFloristsAsync(int? selectedId = null) =>
            await _context.Florists.AsNoTracking().OrderBy(x => x.ArrangementName)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ArrangementName, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
