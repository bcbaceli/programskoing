using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PlaylistController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public PlaylistController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.Playlists.Include(x => x.Band).AsNoTracking();
            query = (sortBy, sortDir) switch
            {
                ("name", "desc") => query.OrderByDescending(x => x.Name),
                ("band", "asc")  => query.OrderBy(x => x.Band.Name),
                ("band", "desc") => query.OrderByDescending(x => x.Band.Name),
                _ => query.OrderBy(x => x.Name)
            };
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            return View(new IndexViewModel<Playlist> { Items = items, CurrentPage = page, TotalPages = (int)Math.Ceiling(total / (double)PageSize), SortBy = sortBy, SortDir = sortDir });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Playlists.Include(x => x.Band).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new PlaylistFormViewModel { Bands = await GetBandsAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(PlaylistFormViewModel vm)
        {
            if (!ModelState.IsValid) { vm.Bands = await GetBandsAsync(); return View(vm); }
            _context.Playlists.Add(new Playlist { Name = vm.Name, BandId = vm.BandId });
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Playlist '{vm.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Playlists.FindAsync(id);
            if (item == null) return NotFound();
            return View(new PlaylistFormViewModel { Id = item.Id, Name = item.Name, BandId = item.BandId, Bands = await GetBandsAsync(item.BandId) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PlaylistFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) { vm.Bands = await GetBandsAsync(vm.BandId); return View(vm); }
            var item = await _context.Playlists.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = vm.Name; item.BandId = vm.BandId;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Playlist '{item.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Playlists.Include(x => x.Band).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Playlists.FindAsync(id);
            if (item != null) { _context.Playlists.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = $"Playlist '{item.Name}' deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetBandsAsync(int? selectedId = null) =>
            await _context.Bands.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
