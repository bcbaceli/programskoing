using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class GenrePlaylistController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public GenrePlaylistController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "genre", string sortDir = "asc", int page = 1)
        {
            var query = _context.GenrePlaylists.Include(x => x.Genre).Include(x => x.Playlist).AsNoTracking();
            query = (sortBy, sortDir) switch
            {
                ("genre",    "desc") => query.OrderByDescending(x => x.Genre.Name),
                ("playlist", "asc")  => query.OrderBy(x => x.Playlist.Name),
                ("playlist", "desc") => query.OrderByDescending(x => x.Playlist.Name),
                _ => query.OrderBy(x => x.Genre.Name)
            };
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
            return View(new IndexViewModel<GenrePlaylist> { Items = items, CurrentPage = page, TotalPages = (int)Math.Ceiling(total / (double)PageSize), SortBy = sortBy, SortDir = sortDir });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.GenrePlaylists.Include(x => x.Genre).Include(x => x.Playlist).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new GenrePlaylistFormViewModel { Genres = await GetGenresAsync(), Playlists = await GetPlaylistsAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(GenrePlaylistFormViewModel vm)
        {
            if (!ModelState.IsValid) { vm.Genres = await GetGenresAsync(); vm.Playlists = await GetPlaylistsAsync(); return View(vm); }
            _context.GenrePlaylists.Add(new GenrePlaylist { GenreId = vm.GenreId, PlaylistId = vm.PlaylistId });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Genre-Playlist link created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.GenrePlaylists.FindAsync(id);
            if (item == null) return NotFound();
            return View(new GenrePlaylistFormViewModel { Id = item.Id, GenreId = item.GenreId, PlaylistId = item.PlaylistId, Genres = await GetGenresAsync(item.GenreId), Playlists = await GetPlaylistsAsync(item.PlaylistId) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GenrePlaylistFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) { vm.Genres = await GetGenresAsync(vm.GenreId); vm.Playlists = await GetPlaylistsAsync(vm.PlaylistId); return View(vm); }
            var item = await _context.GenrePlaylists.FindAsync(id);
            if (item == null) return NotFound();
            item.GenreId = vm.GenreId; item.PlaylistId = vm.PlaylistId;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Genre-Playlist link updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.GenrePlaylists.Include(x => x.Genre).Include(x => x.Playlist).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.GenrePlaylists.FindAsync(id);
            if (item != null) { _context.GenrePlaylists.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = "Genre-Playlist link deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetGenresAsync(int? selectedId = null) =>
            await _context.SongGenres.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();

        private async Task<IEnumerable<SelectListItem>> GetPlaylistsAsync(int? selectedId = null) =>
            await _context.Playlists.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
