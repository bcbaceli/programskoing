using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.API.Models;

namespace WeddingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeddingTemplatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WeddingTemplatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/WeddingTemplates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeddingTemplate>>> GetAll()
        {
            var templates = await _context.WeddingTemplates
                .ToListAsync();

            return Ok(templates);
        }

        // GET: api/WeddingTemplates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeddingTemplate>> GetById(int id)
        {
            var template = await _context.WeddingTemplates.FindAsync(id);

            if (template == null) return NotFound();

            return Ok(template);
        }

        // POST: api/WeddingTemplates
        [HttpPost]
        public async Task<ActionResult<WeddingTemplate>> Create(WeddingTemplate template)
        {
            _context.WeddingTemplates.Add(template);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = template.Id }, template);
        }

        // PUT: api/WeddingTemplates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WeddingTemplate template)
        {
            if (id != template.Id) return BadRequest();

            _context.Entry(template).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.WeddingTemplates.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/WeddingTemplates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _context.WeddingTemplates.FindAsync(id);

            if (template == null) return NotFound();

            _context.WeddingTemplates.Remove(template);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
