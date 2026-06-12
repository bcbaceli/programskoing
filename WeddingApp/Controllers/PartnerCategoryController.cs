using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PartnerCategoryController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public PartnerCategoryController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.PartnerCategories.AsNoTracking();
            query = (sortBy, sortDir) switch
            {
                ("name", "desc") => query.OrderByDescending(x => x.Name),
                _ => query.OrderBy(x => x.Name)
            };
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            return View(new IndexViewModel<PartnerCategory> { Items = items, CurrentPage = page, TotalPages = (int)Math.Ceiling(total / (double)PageSize), SortBy = sortBy, SortDir = sortDir });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.PartnerCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View(new PartnerCategoryFormViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(PartnerCategoryFormViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            _context.PartnerCategories.Add(new PartnerCategory { Name = vm.Name, Description = vm.Description });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Category '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.PartnerCategories.FindAsync(id);
            if (item == null) return NotFound();
            return View(new PartnerCategoryFormViewModel { Id = item.Id, Name = item.Name, Description = item.Description });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PartnerCategoryFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);
            var item = await _context.PartnerCategories.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = vm.Name; item.Description = vm.Description;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Category '{item.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.PartnerCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.PartnerCategories.FindAsync(id);
            if (item != null) { _context.PartnerCategories.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Category '{item.Name}' deleted."; }
            return RedirectToAction(nameof(Index));
        }
    }
}
