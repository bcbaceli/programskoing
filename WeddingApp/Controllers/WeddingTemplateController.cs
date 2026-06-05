
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;

public class WeddingTemplateController : Controller
{
    private readonly AppDbContext _context;

    public WeddingTemplateController(AppDbContext context)
    {
        _context = context;
    }

    // GET: WEDDINGTEMPLATES
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.WeddingTemplates.Include(t => t.Weddings);
        return View(await _context.WeddingTemplates.ToListAsync());
    }

    // GET: WEDDINGTEMPLATES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var weddingtemplate = await _context.WeddingTemplates
            .Include(t => t.Weddings)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (weddingtemplate == null)
        {
            return NotFound();
        }

        return View(weddingtemplate);
    }

    // GET: WEDDINGTEMPLATES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: WEDDINGTEMPLATES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] WeddingTemplate weddingtemplate)
    {
        if (ModelState.IsValid)
        {
            _context.Add(weddingtemplate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(weddingtemplate);
    }

    // GET: WEDDINGTEMPLATES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var weddingtemplate = await _context.WeddingTemplates.FindAsync(id);
        if (weddingtemplate == null)
        {
            return NotFound();
        }
        return View(weddingtemplate);
    }

    // POST: WEDDINGTEMPLATES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Description")] WeddingTemplate weddingtemplate)
    {
        if (id != weddingtemplate.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(weddingtemplate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeddingTemplateExists(weddingtemplate.Id))
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
        return View(weddingtemplate);
    }

    // GET: WEDDINGTEMPLATES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var weddingtemplate = await _context.WeddingTemplates
            .Include(t => t.Weddings)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (weddingtemplate == null)
        {
            return NotFound();
        }

        return View(weddingtemplate);
    }

    // POST: WEDDINGTEMPLATES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var weddingtemplate = await _context.WeddingTemplates.FindAsync(id);
        if (weddingtemplate != null)
        {
            _context.WeddingTemplates.Remove(weddingtemplate);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool WeddingTemplateExists(int? id)
    {
        return _context.WeddingTemplates.Any(e => e.Id == id);
    }
}