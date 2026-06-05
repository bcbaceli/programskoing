
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Models;
using WeddingApp.Data;

public class FoodMenuController : Controller
{
    private readonly AppDbContext _context;

    public FoodMenuController(AppDbContext context)
    {
        _context = context;
    }

    // GET: FOODMENUS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.FoodMenus.ToListAsync());
    }

    // GET: FOODMENUS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var foodmenu = await _context.FoodMenus
            .FirstOrDefaultAsync(m => m.Id == id);
        if (foodmenu == null)
        {
            return NotFound();
        }

        return View(foodmenu);
    }

    // GET: FOODMENUS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: FOODMENUS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,PricePerPerson,RestaurantId,Restaurant")] FoodMenu foodmenu)
    {
        if (ModelState.IsValid)
        {
            _context.Add(foodmenu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(foodmenu);
    }

    // GET: FOODMENUS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var foodmenu = await _context.FoodMenus.FindAsync(id);
        if (foodmenu == null)
        {
            return NotFound();
        }
        return View(foodmenu);
    }

    // POST: FOODMENUS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,PricePerPerson,RestaurantId,Restaurant")] FoodMenu foodmenu)
    {
        if (id != foodmenu.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(foodmenu);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodMenuExists(foodmenu.Id))
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
        return View(foodmenu);
    }

    // GET: FOODMENUS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var foodmenu = await _context.FoodMenus
            .FirstOrDefaultAsync(m => m.Id == id);
        if (foodmenu == null)
        {
            return NotFound();
        }

        return View(foodmenu);
    }

    // POST: FOODMENUS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var foodmenu = await _context.FoodMenus.FindAsync(id);
        if (foodmenu != null)
        {
            _context.FoodMenus.Remove(foodmenu);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool FoodMenuExists(int? id)
    {
        return _context.FoodMenus.Any(e => e.Id == id);
    }
}
