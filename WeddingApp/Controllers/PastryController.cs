
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;

public class PastryController : Controller
{
    private readonly AppDbContext _context;

    public PastryController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PASTRYS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Pastries.ToListAsync());
    }

    // GET: PASTRYS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pastry = await _context.Pastries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pastry == null)
        {
            return NotFound();
        }

        return View(pastry);
    }

    // GET: PASTRYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PASTRYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,ItemId,Item,PartnerId,Partner")] Pastry pastry)
    {
        if (ModelState.IsValid)
        {
            _context.Add(pastry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(pastry);
    }

    // GET: PASTRYS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pastry = await _context.Pastries.FindAsync(id);
        if (pastry == null)
        {
            return NotFound();
        }
        return View(pastry);
    }

    // POST: PASTRYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,ItemId,Item,PartnerId,Partner")] Pastry pastry)
    {
        if (id != pastry.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pastry);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PastryExists(pastry.Id))
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
        return View(pastry);
    }

    // GET: PASTRYS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pastry = await _context.Pastries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pastry == null)
        {
            return NotFound();
        }

        return View(pastry);
    }

    // POST: PASTRYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var pastry = await _context.Pastries.FindAsync(id);
        if (pastry != null)
        {
            _context.Pastries.Remove(pastry);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PastryExists(int? id)
    {
        return _context.Pastries.Any(e => e.Id == id);
    }
}
