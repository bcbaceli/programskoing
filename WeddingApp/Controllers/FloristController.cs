using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class FloristController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public FloristController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.Florists.Include(x => x.Item).Include(x => x.Partner).AsNoTracking();
            query = (sortBy, sortDir) switch
            {
                ("name",    "desc") => query.OrderByDescending(x => x.ArrangementName),
                ("item",    "asc")  => query.OrderBy(x => x.Item.Name),
                ("item",    "desc") => query.OrderByDescending(x => x.Item.Name),
                ("partner", "asc")  => query.OrderBy(x => x.Partner.Name),
                ("partner", "desc") => query.OrderByDescending(x => x.Partner.Name),
                _ => query.OrderBy(x => x.ArrangementName)
            };
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            return View(new IndexViewModel<Florist> { Items = items, CurrentPage = page, TotalPages = (int)Math.Ceiling(total / (double)PageSize), SortBy = sortBy, SortDir = sortDir });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Florists.Include(x => x.Item).Include(x => x.Partner).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new FloristFormViewModel { Items = await GetItemsAsync(), Partners = await GetPartnersAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(FloristFormViewModel vm)
        {
            if (!ModelState.IsValid) { vm.Items = await GetItemsAsync(); vm.Partners = await GetPartnersAsync(); return View(vm); }
            _context.Florists.Add(new Florist { ArrangementName = vm.ArrangementName, ItemId = vm.ItemId, PartnerId = vm.PartnerId });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Florist '{vm.ArrangementName}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Florists.FindAsync(id);
            if (item == null) return NotFound();
            return View(new FloristFormViewModel { Id = item.Id, ArrangementName = item.ArrangementName, ItemId = item.ItemId, PartnerId = item.PartnerId, Items = await GetItemsAsync(item.ItemId), Partners = await GetPartnersAsync(item.PartnerId) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FloristFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) { vm.Items = await GetItemsAsync(vm.ItemId); vm.Partners = await GetPartnersAsync(vm.PartnerId); return View(vm); }
            var item = await _context.Florists.FindAsync(id);
            if (item == null) return NotFound();
            item.ArrangementName = vm.ArrangementName; item.ItemId = vm.ItemId; item.PartnerId = vm.PartnerId;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Florist '{item.ArrangementName}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Florists.Include(x => x.Item).Include(x => x.Partner).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Florists.FindAsync(id);
            if (item != null) { _context.Florists.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Florist '{item.ArrangementName}' deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetItemsAsync(int? selectedId = null) =>
            await _context.Items.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();

        private async Task<IEnumerable<SelectListItem>> GetPartnersAsync(int? selectedId = null) =>
            await _context.Partners.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
