
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;

public class GenrePlaylistController : Controller
{
    private readonly AppDbContext _context;

    public GenrePlaylistController(AppDbContext context)
    {
        _context = context;
    }

    // GET: GENREPLAYLISTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.GenrePlaylists.ToListAsync());
    }

    // GET: GENREPLAYLISTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genreplaylist = await _context.GenrePlaylists
            .FirstOrDefaultAsync(m => m.Id == id);
        if (genreplaylist == null)
        {
            return NotFound();
        }

        return View(genreplaylist);
    }

    // GET: GENREPLAYLISTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: GENREPLAYLISTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,GenreId,Genre,PlaylistId,Playlist")] GenrePlaylist genreplaylist)
    {
        if (ModelState.IsValid)
        {
            _context.Add(genreplaylist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(genreplaylist);
    }

    // GET: GENREPLAYLISTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genreplaylist = await _context.GenrePlaylists.FindAsync(id);
        if (genreplaylist == null)
        {
            return NotFound();
        }
        return View(genreplaylist);
    }

    // POST: GENREPLAYLISTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,GenreId,Genre,PlaylistId,Playlist")] GenrePlaylist genreplaylist)
    {
        if (id != genreplaylist.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(genreplaylist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenrePlaylistExists(genreplaylist.Id))
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
        return View(genreplaylist);
    }

    // GET: GENREPLAYLISTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var genreplaylist = await _context.GenrePlaylists
            .FirstOrDefaultAsync(m => m.Id == id);
        if (genreplaylist == null)
        {
            return NotFound();
        }

        return View(genreplaylist);
    }

    // POST: GENREPLAYLISTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var genreplaylist = await _context.GenrePlaylists.FindAsync(id);
        if (genreplaylist != null)
        {
            _context.GenrePlaylists.Remove(genreplaylist);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GenrePlaylistExists(int? id)
    {
        return _context.GenrePlaylists.Any(e => e.Id == id);
    }
}
