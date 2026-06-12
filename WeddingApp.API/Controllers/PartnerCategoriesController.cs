using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.API.Models;

namespace WeddingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartnerCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PartnerCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnerCategory>>> GetAll()
        {
            var categories = await _context.PartnerCategories
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/PartnerCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PartnerCategory>> GetById(int id)
        {
            var category = await _context.PartnerCategories
                .Include(c => c.Partners)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();

            return Ok(category);
        }

        // POST: api/PartnerCategories
        [HttpPost]
        public async Task<ActionResult<PartnerCategory>> Create(PartnerCategory category)
        {
            _context.PartnerCategories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // PUT: api/PartnerCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PartnerCategory category)
        {
            if (id != category.Id) return BadRequest();

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PartnerCategories.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/PartnerCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.PartnerCategories.FindAsync(id);
            if (category == null) return NotFound();

            _context.PartnerCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
