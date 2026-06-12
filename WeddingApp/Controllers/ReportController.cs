using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data;
using WeddingApp.Models;
using WeddingApp.ViewModels;

namespace WeddingApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public ReportController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string sortBy = "partner", string sortDir = "asc", int page = 1)
        {
            var query = _context.Reports
                .Include(x => x.Partner)
                .Include(x => x.Wedding).ThenInclude(x => x.Wedding)
                .AsNoTracking();

            query = (sortBy, sortDir) switch
            {
                ("partner", "desc") => query.OrderByDescending(x => x.Partner.Name),
                ("wedding", "asc")  => query.OrderBy(x => x.Wedding.Wedding.Date),
                ("wedding", "desc") => query.OrderByDescending(x => x.Wedding.Wedding.Date),
                _ => query.OrderBy(x => x.Partner.Name)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            return View(new IndexViewModel<Report>
            {
                Items = items, CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)PageSize),
                SortBy = sortBy, SortDir = sortDir
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Reports
                .Include(x => x.Partner)
                .Include(x => x.Wedding).ThenInclude(x => x.Wedding)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create() =>
            View(new ReportFormViewModel { Partners = await GetPartnersAsync(), WeddingCeremonies = await GetCeremoniesAsync() });

        [HttpPost]
        public async Task<IActionResult> Create(ReportFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Partners = await GetPartnersAsync(); vm.WeddingCeremonies = await GetCeremoniesAsync();
                return View(vm);
            }
            _context.Reports.Add(new Report { PartnerId = vm.PartnerId, WeddingId = vm.WeddingId });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Report created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Reports.FindAsync(id);
            if (item == null) return NotFound();
            return View(new ReportFormViewModel
            {
                Id = item.Id, PartnerId = item.PartnerId, WeddingId = item.WeddingId,
                Partners = await GetPartnersAsync(item.PartnerId),
                WeddingCeremonies = await GetCeremoniesAsync(item.WeddingId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReportFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                vm.Partners = await GetPartnersAsync(vm.PartnerId); vm.WeddingCeremonies = await GetCeremoniesAsync(vm.WeddingId);
                return View(vm);
            }
            var item = await _context.Reports.FindAsync(id);
            if (item == null) return NotFound();

            item.PartnerId = vm.PartnerId; item.WeddingId = vm.WeddingId;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Report updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Reports
                .Include(x => x.Partner)
                .Include(x => x.Wedding).ThenInclude(x => x.Wedding)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Reports.FindAsync(id);
            if (item != null) { _context.Reports.Remove(item); await _context.SaveChangesAsync(); TempData["Success"] = "Report deleted."; }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetPartnersAsync(int? selectedId = null) =>
            await _context.Partners.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == selectedId })
                .ToListAsync();

        private async Task<IEnumerable<SelectListItem>> GetCeremoniesAsync(int? selectedId = null) =>
            await _context.WeddingCeremonies.Include(x => x.Wedding).AsNoTracking()
                .OrderByDescending(x => x.Wedding.Date)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Wedding.Date.ToString("d") + " — " + x.Restaurant.Name, Selected = x.Id == selectedId })
                .ToListAsync();
    }
}
