using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class WeddingController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public WeddingController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "date", string sortDir = "asc", int page = 1)
        {
            var query = _context.Weddings.Include(x => x.Template).AsNoTracking();
            query = (sortBy, sortDir) switch
            {
                ("date",     "desc") => query.OrderByDescending(x => x.Date),
                ("address",  "asc")  => query.OrderBy(x => x.Address),
                ("address",  "desc") => query.OrderByDescending(x => x.Address),
                ("template", "asc")  => query.OrderBy(x => x.Template.Name),
                ("template", "desc") => query.OrderByDescending(x => x.Template.Name),
                _ => query.OrderBy(x => x.Date)
            };
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            return View(new IndexViewModel<Wedding> { Items = items, CurrentPage = page, TotalPages = (int)Math.Ceiling(total / (double)PageSize), SortBy = sortBy, SortDir = sortDir });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Weddings.Include(x => x.Template).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new WeddingFormViewModel { Templates = await GetTemplatesAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(WeddingFormViewModel vm)
        {
            if (!ModelState.IsValid) { vm.Templates = await GetTemplatesAsync(); return View(vm); }
            _context.Weddings.Add(new Wedding { Date = vm.Date, Address = vm.Address, Phone = vm.Phone, Email = vm.Email, TemplateId = vm.TemplateId });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Wedding on {vm.Date:d} created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Weddings.FindAsync(id);
            if (item == null) return NotFound();
            return View(new WeddingFormViewModel { Id = item.Id, Date = item.Date, Address = item.Address, Phone = item.Phone, Email = item.Email, TemplateId = item.TemplateId, Templates = await GetTemplatesAsync(item.TemplateId) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, WeddingFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) { vm.Templates = await GetTemplatesAsync(vm.TemplateId); return View(vm); }
            var item = await _context.Weddings.FindAsync(id);
            if (item == null) return NotFound();
            item.Date = vm.Date; item.Address = vm.Address; item.Phone = vm.Phone; item.Email = vm.Email; item.TemplateId = vm.TemplateId;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Wedding on {item.Date:d} updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Weddings.Include(x => x.Template).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Weddings.FindAsync(id);
            if (item != null) { _context.Weddings.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Wedding on {item.Date:d} deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetTemplatesAsync(int? selectedId = null) =>
            await _context.WeddingTemplates.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
