using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using FleetManagement.Data;
using FleetManagement.Models;

namespace FleetManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccidentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccidentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/accidents
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Accidents
                .Include(a => a.Vehicle)
                .Include(a => a.Driver)
                .ToListAsync();

            return Ok(data);
        }

        // GET: api/accidents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.Accidents
                .Include(a => a.Vehicle)
                .Include(a => a.Driver)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/accidents
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Accident a)
        {
            _context.Accidents.Add(a);
            await _context.SaveChangesAsync();

            return Ok(a);
        }

        // PUT: api/accidents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Accident a)
        {
            if (id != a.Id)
                return BadRequest();

            _context.Entry(a).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(a);
        }

        // DELETE: api/accidents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _context.Accidents.FindAsync(id);

            if (a == null)
                return NotFound();

            _context.Accidents.Remove(a);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}