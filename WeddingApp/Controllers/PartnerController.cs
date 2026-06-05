using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PartnerController : Controller
{
    private readonly AppDbContext _context;

    public PartnerController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PARTNERS
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Partners.Include(p => p.Category);
        return View(await appDbContext.ToListAsync());
    }

    // GET: PARTNERS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var partner = await _context.Partners
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (partner == null)
        {
            return NotFound();
        }

        return View(partner);
    }

    // GET: PARTNERS/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.PartnerCategories, "Id", "Name");
        return View();
    }

    // POST: PARTNERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Address,Phone,Email,CommissionPct,Category")] Partner partner)
    {
        if (ModelState.IsValid)
        {
            _context.Add(partner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.PartnerCategories, "Id", "Name", partner.CategoryId);
        return View(partner);
    }

    // GET: PARTNERS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var partner = await _context.Partners.FindAsync(id);
        if (partner == null)
        {
            return NotFound();
        }
        ViewData["CategoryId"] = new SelectList(_context.PartnerCategories, "Id", "Name", partner.CategoryId);
        return View(partner);
    }

    // POST: PARTNERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,CategoryId,Name,Address,Phone,Email,CommissionPct,Category")] Partner partner)
    {
        if (id != partner.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(partner);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerExists(partner.Id))
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
        return View(partner);
    }

    // GET: PARTNERS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var partner = await _context.Partners
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (partner == null)
        {
            return NotFound();
        }

        return View(partner);
    }

    // POST: PARTNERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var partner = await _context.Partners.FindAsync(id);
        if (partner != null)
        {
            _context.Partners.Remove(partner);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PartnerExists(int? id)
    {
        return _context.Partners.Any(e => e.Id == id);
    }
}