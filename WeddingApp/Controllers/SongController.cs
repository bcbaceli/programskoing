using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SongController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public SongController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.Songs.Include(x => x.Genre).AsNoTracking();

            query = (sortBy, sortDir) switch
            {
                ("name",  "desc") => query.OrderByDescending(x => x.Name),
                ("genre", "asc")  => query.OrderBy(x => x.Genre.Name),
                ("genre", "desc") => query.OrderByDescending(x => x.Genre.Name),
                _ => query.OrderBy(x => x.Name)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            return View(new IndexViewModel<Song>
            {
                Items = items, CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)PageSize),
                SortBy = sortBy, SortDir = sortDir
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Songs.Include(x => x.Genre).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new SongFormViewModel { Genres = await GetGenresAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(SongFormViewModel vm)
        {
            if (!ModelState.IsValid) { vm.Genres = await GetGenresAsync(); return View(vm); }

            _context.Songs.Add(new Song { Name = vm.Name, GenreId = vm.GenreId });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Song '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Songs.FindAsync(id);
            if (item == null) return NotFound();
            return View(new SongFormViewModel
            {
                Id = item.Id, Name = item.Name, GenreId = item.GenreId,
                Genres = await GetGenresAsync(item.GenreId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SongFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) { vm.Genres = await GetGenresAsync(vm.GenreId); return View(vm); }

            var item = await _context.Songs.FindAsync(id);
            if (item == null) return NotFound();

            item.Name = vm.Name; item.GenreId = vm.GenreId;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Song '{item.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Songs.Include(x => x.Genre).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Songs.FindAsync(id);
            if (item != null) { _context.Songs.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Song '{item.Name}' deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetGenresAsync(int? selectedId = null) =>
            await _context.SongGenres.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
