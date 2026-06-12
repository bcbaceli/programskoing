using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PartnerController : Controller
    {
        private readonly AppDbContext _context;

        public PartnerController(AppDbContext context)
        {
            _context = context;
        }

        private const int PageSize = 10;

        public async Task<IActionResult> Index(string sortBy = "name", string sortDir = "asc", int page = 1)
        {
            var query = _context.Partners
                .Include(p => p.Category)
                .AsNoTracking();

            query = (sortBy, sortDir) switch
            {
                ("name",       "desc") => query.OrderByDescending(p => p.Name),
                ("category",   "asc")  => query.OrderBy(p => p.Category!.Name),
                ("category",   "desc") => query.OrderByDescending(p => p.Category!.Name),
                ("commission", "asc")  => query.OrderBy(p => p.CommissionPct),
                ("commission", "desc") => query.OrderByDescending(p => p.CommissionPct),
                _                      => query.OrderBy(p => p.Name)
            };

            var total = await query.CountAsync();
            var partners = await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var vm = new IndexViewModel<Partner>
            {
                Items = partners,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)PageSize),
                SortBy = sortBy,
                SortDir = sortDir
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var partner = await _context.Partners
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partner == null)
                return NotFound();

            return View(partner);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new PartnerFormViewModel
            {
                Categories = await GetCategoriesAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PartnerFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await GetCategoriesAsync();
                return View(vm);
            }

            var partner = new Partner
            {
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Address = vm.Address,
                Phone = vm.Phone,
                Email = vm.Email,
                CommissionPct = vm.CommissionPct
            };

            _context.Partners.Add(partner);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Partner '{partner.Name}' created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var partner = await _context.Partners.FindAsync(id);

            if (partner == null)
                return NotFound();

            var vm = new PartnerFormViewModel
            {
                Id = partner.Id,
                CategoryId = partner.CategoryId,
                Name = partner.Name,
                Address = partner.Address,
                Phone = partner.Phone,
                Email = partner.Email,
                CommissionPct = partner.CommissionPct,
                Categories = await GetCategoriesAsync(partner.CategoryId)
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PartnerFormViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Categories = await GetCategoriesAsync(vm.CategoryId);
                return View(vm);
            }

            var partner = await _context.Partners.FindAsync(id);

            if (partner == null)
                return NotFound();

            partner.CategoryId = vm.CategoryId;
            partner.Name = vm.Name;
            partner.Address = vm.Address;
            partner.Phone = vm.Phone;
            partner.Email = vm.Email;
            partner.CommissionPct = vm.CommissionPct;

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Partner '{partner.Name}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var partner = await _context.Partners
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partner == null)
                return NotFound();

            return View(partner);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partner = await _context.Partners.FindAsync(id);

            if (partner != null)
            {
                _context.Partners.Remove(partner);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Partner '{partner.Name}' deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetCategoriesAsync(int? selectedId = null)
        {
            return await _context.PartnerCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == selectedId
                })
                .ToListAsync();
        }
    }
}
