
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;

public class PartnerCategoryController : Controller
{
    private readonly AppDbContext _context;

    public PartnerCategoryController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PARTNERCATEGORYS
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.PartnerCategories.Include(c => c.Partners);
        return View(await appDbContext.ToListAsync());
    }

    // GET: PARTNERCATEGORYS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var partnercategory = await _context.PartnerCategories
            .Include(c => c.Partners)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (partnercategory == null)
        {
            return NotFound();
        }

        return View(partnercategory);
    }

    // GET: PARTNERCATEGORYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PARTNERCATEGORYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] PartnerCategory partnercategory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(partnercategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(partnercategory);
    }

    // GET: PARTNERCATEGORYS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var partnercategory = await _context.PartnerCategories.FindAsync(id);
        if (partnercategory == null)
        {
            return NotFound();
        }
        return View(partnercategory);
    }

    // POST: PARTNERCATEGORYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Description")] PartnerCategory partnercategory)
    {
        if (id != partnercategory.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(partnercategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerCategoryExists(partnercategory.Id))
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
        return View(partnercategory);
    }

    // GET: PARTNERCATEGORYS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var partnercategory = await _context.PartnerCategories
            .Include(c => c.Partners)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (partnercategory == null)
        {
            return NotFound();
        }

        return View(partnercategory);
    }

    // POST: PARTNERCATEGORYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var partnercategory = await _context.PartnerCategories.FindAsync(id);
        if (partnercategory != null)
        {
            _context.PartnerCategories.Remove(partnercategory);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PartnerCategoryExists(int? id)
    {
        return _context.PartnerCategories.Any(e => e.Id == id);
    }
}