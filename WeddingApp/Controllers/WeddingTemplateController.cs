using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class WeddingTemplateController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public WeddingTemplateController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.WeddingTemplates.AsNoTracking();
            query = (sortBy, sortDir) switch
            {
                ("name", "desc") => query.OrderByDescending(x => x.Name),
                _ => query.OrderBy(x => x.Name)
            };
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            return View(new IndexViewModel<WeddingTemplate> { Items = items, CurrentPage = page, TotalPages = (int)Math.Ceiling(total / (double)PageSize), SortBy = sortBy, SortDir = sortDir });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.WeddingTemplates.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View(new WeddingTemplateFormViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(WeddingTemplateFormViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            _context.WeddingTemplates.Add(new WeddingTemplate { Name = vm.Name, Description = vm.Description });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Template '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.WeddingTemplates.FindAsync(id);
            if (item == null) return NotFound();
            return View(new WeddingTemplateFormViewModel { Id = item.Id, Name = item.Name, Description = item.Description });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, WeddingTemplateFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);
            var item = await _context.WeddingTemplates.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = vm.Name; item.Description = vm.Description;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Template '{item.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.WeddingTemplates.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.WeddingTemplates.FindAsync(id);
            if (item != null) { _context.WeddingTemplates.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Template '{item.Name}' deleted."; }
            return RedirectToAction(nameof(Index));
        }
    }
}
