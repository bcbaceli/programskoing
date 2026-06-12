using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingApp.API.Models;

namespace WeddingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartnersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Partners
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnerDto>>> GetAll()
        {
            var partners = await _context.Partners
                .Include(p => p.Category)
                .Select(p => new PartnerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    Phone = p.Phone,
                    Email = p.Email,
                    CommissionPct = p.CommissionPct,
                    CategoryName = p.Category != null ? p.Category.Name : null
                })
                .ToListAsync();

            return Ok(partners);
        }

        // GET: api/Partners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PartnerDto>> GetById(int id)
        {
            var partner = await _context.Partners
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partner == null) return NotFound();

            var dto = new PartnerDto
            {
                Id = partner.Id,
                Name = partner.Name,
                Address = partner.Address,
                Phone = partner.Phone,
                Email = partner.Email,
                CommissionPct = partner.CommissionPct,
                CategoryName = partner.Category?.Name
            };

            return Ok(dto);
        }

        // POST: api/Partners
        [HttpPost]
        public async Task<ActionResult<PartnerDto>> Create(Partner partner)
        {
            _context.Partners.Add(partner);
            await _context.SaveChangesAsync();

            var dto = new PartnerDto
            {
                Id = partner.Id,
                Name = partner.Name,
                Address = partner.Address,
                Phone = partner.Phone,
                Email = partner.Email,
                CommissionPct = partner.CommissionPct
            };

            return CreatedAtAction(nameof(GetById), new { id = partner.Id }, dto);
        }

        // PUT: api/Partners/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Partner partner)
        {
            if (id != partner.Id) return BadRequest();

            _context.Entry(partner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Partners.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Partners/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var partner = await _context.Partners.FindAsync(id);
            if (partner == null) return NotFound();

            _context.Partners.Remove(partner);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
