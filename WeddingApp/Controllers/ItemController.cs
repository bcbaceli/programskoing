using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public ItemController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.Items.AsNoTracking();
            query = (sortBy, sortDir) switch
            {
                ("name", "desc") => query.OrderByDescending(x => x.Name),
                _ => query.OrderBy(x => x.Name)
            };
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            return View(new IndexViewModel<Item> { Items = items, CurrentPage = page, TotalPages = (int)Math.Ceiling(total / (double)PageSize), SortBy = sortBy, SortDir = sortDir });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View(new ItemFormViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(ItemFormViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            _context.Items.Add(new Item { Name = vm.Name });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Item '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return NotFound();
            return View(new ItemFormViewModel { Id = item.Id, Name = item.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ItemFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);
            var item = await _context.Items.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = vm.Name;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Item '{item.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null) { _context.Items.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Item '{item.Name}' deleted."; }
            return RedirectToAction(nameof(Index));
        }
    }
}
