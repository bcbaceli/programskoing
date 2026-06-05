
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

public class WeddingController : Controller
{
    private readonly AppDbContext _context;

    public WeddingController(AppDbContext context)
    {
        _context = context;
    }

    // GET: WEDDINGS
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Weddings.Include(w => w.Template);
        return View(await appDbContext.ToListAsync());
    }

    // GET: WEDDINGS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var wedding = await _context.Weddings
            .Include(w => w.Template)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (wedding == null)
        {
            return NotFound();
        }

        return View(wedding);
    }

    // GET: WEDDINGS/Create
    public IActionResult Create()
    {
        ViewData["TemplateId"] = new SelectList(_context.WeddingTemplates, "Id", "Name");
        return View();
    }

    // POST: WEDDINGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Date,Address,Phone,Email,TemplateId,Template")] Wedding wedding)
    {
        if (ModelState.IsValid)
        {
            _context.Add(wedding);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["TemplateId"] = new SelectList(_context.WeddingTemplates, "Id", "Name", wedding.TemplateId);
        return View(wedding);
    }

    // GET: WEDDINGS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var wedding = await _context.Weddings.FindAsync(id);
        if (wedding == null)
        {
            return NotFound();
        }
        ViewData["TemplateId"] = new SelectList(_context.WeddingTemplates, "Id", "Name", wedding.TemplateId);
        return View(wedding);
    }

    // POST: WEDDINGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Date,Address,Phone,Email,TemplateId,Template")] Wedding wedding)
    {
        if (id != wedding.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(wedding);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeddingExists(wedding.Id))
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
        return View(wedding);
    }

    // GET: WEDDINGS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var wedding = await _context.Weddings
            .Include(w => w.Template)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (wedding == null)
        {
            return NotFound();
        }

        return View(wedding);
    }

    // POST: WEDDINGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var wedding = await _context.Weddings.FindAsync(id);
        if (wedding != null)
        {
            _context.Weddings.Remove(wedding);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool WeddingExists(int? id)
    {
        return _context.Weddings.Any(e => e.Id == id);
    }
}