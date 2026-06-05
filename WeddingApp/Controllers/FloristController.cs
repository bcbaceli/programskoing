
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;

public class FloristController : Controller
{
    private readonly AppDbContext _context;

    public FloristController(AppDbContext context)
    {
        _context = context;
    }

    // GET: FLORISTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Florists.ToListAsync());
    }

    // GET: FLORISTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var florist = await _context.Florists
            .FirstOrDefaultAsync(m => m.Id == id);
        if (florist == null)
        {
            return NotFound();
        }

        return View(florist);
    }

    // GET: FLORISTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: FLORISTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ArrangementName,ItemId,Item,PartnerId,Partner,WeddingCeremonies")] Florist florist)
    {
        if (ModelState.IsValid)
        {
            _context.Add(florist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(florist);
    }

    // GET: FLORISTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var florist = await _context.Florists.FindAsync(id);
        if (florist == null)
        {
            return NotFound();
        }
        return View(florist);
    }

    // POST: FLORISTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,ArrangementName,ItemId,Item,PartnerId,Partner,WeddingCeremonies")] Florist florist)
    {
        if (id != florist.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(florist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FloristExists(florist.Id))
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
        return View(florist);
    }

    // GET: FLORISTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var florist = await _context.Florists
            .FirstOrDefaultAsync(m => m.Id == id);
        if (florist == null)
        {
            return NotFound();
        }

        return View(florist);
    }

    // POST: FLORISTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var florist = await _context.Florists.FindAsync(id);
        if (florist != null)
        {
            _context.Florists.Remove(florist);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool FloristExists(int? id)
    {
        return _context.Florists.Any(e => e.Id == id);
    }
}
