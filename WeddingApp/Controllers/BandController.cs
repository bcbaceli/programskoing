
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;

public class BandController : Controller
{
    private readonly AppDbContext _context;

    public BandController(AppDbContext context)
    {
        _context = context;
    }

    // GET: BANDS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Bands.ToListAsync());
    }

    // GET: BANDS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var band = await _context.Bands
            .FirstOrDefaultAsync(m => m.Id == id);
        if (band == null)
        {
            return NotFound();
        }

        return View(band);
    }

    // GET: BANDS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: BANDS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,DayAtWeek,HoursPerWedding,TotalPrice,PartnerId,Partner,WeddingCeremonies,Bands")] Band band)
    {
        if (ModelState.IsValid)
        {
            _context.Add(band);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(band);
    }

    // GET: BANDS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var band = await _context.Bands.FindAsync(id);
        if (band == null)
        {
            return NotFound();
        }
        return View(band);
    }

    // POST: BANDS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,DayAtWeek,HoursPerWedding,TotalPrice,PartnerId,Partner,WeddingCeremonies,Bands")] Band band)
    {
        if (id != band.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(band);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BandExists(band.Id))
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
        return View(band);
    }

    // GET: BANDS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var band = await _context.Bands
            .FirstOrDefaultAsync(m => m.Id == id);
        if (band == null)
        {
            return NotFound();
        }

        return View(band);
    }

    // POST: BANDS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var band = await _context.Bands.FindAsync(id);
        if (band != null)
        {
            _context.Bands.Remove(band);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BandExists(int? id)
    {
        return _context.Bands.Any(e => e.Id == id);
    }
}
