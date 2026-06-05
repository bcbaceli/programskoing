
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;

public class PlaylistController : Controller
{
    private readonly AppDbContext _context;

    public PlaylistController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PLAYLISTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Playlists.ToListAsync());
    }

    // GET: PLAYLISTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(m => m.Id == id);
        if (playlist == null)
        {
            return NotFound();
        }

        return View(playlist);
    }

    // GET: PLAYLISTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PLAYLISTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,BandId,Band,GenrePlaylists,SongPlaylists")] Playlist playlist)
    {
        if (ModelState.IsValid)
        {
            _context.Add(playlist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(playlist);
    }

    // GET: PLAYLISTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist == null)
        {
            return NotFound();
        }
        return View(playlist);
    }

    // POST: PLAYLISTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,BandId,Band,GenrePlaylists,SongPlaylists")] Playlist playlist)
    {
        if (id != playlist.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(playlist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistExists(playlist.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(playlist);
    }

    // GET: PLAYLISTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(m => m.Id == id);
        if (playlist == null)
        {
            return NotFound();
        }

        return View(playlist);
    }

    // POST: PLAYLISTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist != null)
        {
            _context.Playlists.Remove(playlist);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PlaylistExists(int? id)
    {
        return _context.Playlists.Any(e => e.Id == id);
    }
}
