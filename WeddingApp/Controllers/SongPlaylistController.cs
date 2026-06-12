using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SongPlaylistController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public SongPlaylistController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "song", string sortDir = "asc", int page = 1)
        {
            var query = _context.SongPlaylists.Include(x => x.Song).Include(x => x.Playlist).AsNoTracking();

            query = (sortBy, sortDir) switch
            {
                ("song",     "desc") => query.OrderByDescending(x => x.Song.Name),
                ("playlist", "asc")  => query.OrderBy(x => x.Playlist.Name),
                ("playlist", "desc") => query.OrderByDescending(x => x.Playlist.Name),
                _ => query.OrderBy(x => x.Song.Name)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            return View(new IndexViewModel<SongPlaylist>
            {
                Items = items, CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)PageSize),
                SortBy = sortBy, SortDir = sortDir
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.SongPlaylists.Include(x => x.Song).Include(x => x.Playlist).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new SongPlaylistFormViewModel { Songs = await GetSongsAsync(), Playlists = await GetPlaylistsAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(SongPlaylistFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Songs = await GetSongsAsync(); vm.Playlists = await GetPlaylistsAsync();
                return View(vm);
            }
            _context.SongPlaylists.Add(new SongPlaylist { SongId = vm.SongId, PlaylistId = vm.PlaylistId });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Song-Playlist link created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.SongPlaylists.FindAsync(id);
            if (item == null) return NotFound();
            return View(new SongPlaylistFormViewModel
            {
                Id = item.Id, SongId = item.SongId, PlaylistId = item.PlaylistId,
                Songs = await GetSongsAsync(item.SongId), Playlists = await GetPlaylistsAsync(item.PlaylistId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SongPlaylistFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                vm.Songs = await GetSongsAsync(vm.SongId); vm.Playlists = await GetPlaylistsAsync(vm.PlaylistId);
                return View(vm);
            }
            var item = await _context.SongPlaylists.FindAsync(id);
            if (item == null) return NotFound();

            item.SongId = vm.SongId; item.PlaylistId = vm.PlaylistId;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Song-Playlist link updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.SongPlaylists.Include(x => x.Song).Include(x => x.Playlist).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.SongPlaylists.FindAsync(id);
            if (item != null) { _context.SongPlaylists.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = "Song-Playlist link deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetSongsAsync(int? selectedId = null) =>
            await _context.Songs.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();

        private async Task<IEnumerable<SelectListItem>> GetPlaylistsAsync(int? selectedId = null) =>
            await _context.Playlists.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
