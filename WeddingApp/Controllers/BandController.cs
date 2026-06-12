using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class BandController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public BandController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.Bands.Include(x => x.Partner).AsNoTracking();
            query = (sortBy, sortDir) switch
            {
                ("name",    "desc") => query.OrderByDescending(x => x.Name),
                ("partner", "asc")  => query.OrderBy(x => x.Partner.Name),
                ("partner", "desc") => query.OrderByDescending(x => x.Partner.Name),
                ("price",   "asc")  => query.OrderBy(x => x.TotalPrice),
                ("price",   "desc") => query.OrderByDescending(x => x.TotalPrice),
                _ => query.OrderBy(x => x.Name)
            };
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            return View(new IndexViewModel<Band> { Items = items, CurrentPage = page, TotalPages = (int)Math.Ceiling(total / (double)PageSize), SortBy = sortBy, SortDir = sortDir });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Bands.Include(x => x.Partner).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new BandFormViewModel { Partners = await GetPartnersAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(BandFormViewModel vm)
        {
            if (!ModelState.IsValid) { vm.Partners = await GetPartnersAsync(); return View(vm); }
            _context.Bands.Add(new Band { Name = vm.Name, DayAtWeek = vm.DayAtWeek, HoursPerWedding = vm.HoursPerWedding, TotalPrice = vm.TotalPrice, PartnerId = vm.PartnerId });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Band '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Bands.FindAsync(id);
            if (item == null) return NotFound();
            return View(new BandFormViewModel { Id = item.Id, Name = item.Name, DayAtWeek = item.DayAtWeek, HoursPerWedding = item.HoursPerWedding, TotalPrice = item.TotalPrice, PartnerId = item.PartnerId, Partners = await GetPartnersAsync(item.PartnerId) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BandFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) { vm.Partners = await GetPartnersAsync(vm.PartnerId); return View(vm); }
            var item = await _context.Bands.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = vm.Name; item.DayAtWeek = vm.DayAtWeek; item.HoursPerWedding = vm.HoursPerWedding; item.TotalPrice = vm.TotalPrice; item.PartnerId = vm.PartnerId;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Band '{item.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Bands.Include(x => x.Partner).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Bands.FindAsync(id);
            if (item != null) { _context.Bands.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Band '{item.Name}' deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetPartnersAsync(int? selectedId = null) =>
            await _context.Partners.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
