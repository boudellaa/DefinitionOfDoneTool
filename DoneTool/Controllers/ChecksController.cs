using Microsoft.AspNetCore.Mvc;
using DoneTool.Models;
using DoneTool.Data;
using Microsoft.EntityFrameworkCore;

namespace DoneTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecksController : ControllerBase
    {
        private readonly DoneToolContext _context;

        public ChecksController(DoneToolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Checks>>> GetChecksItems()
        {
            return await _context.Checks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Checks>> GetChecksItem(Guid id)
        {
            var checksItem = await _context.Checks.FindAsync(id);

            if (checksItem == null)
            {
                return NotFound();
            }

            return checksItem;
        }

        [HttpPost]
        public async Task<ActionResult<Checks>> PostChecksItem(Checks checksItem)
        {
            _context.Checks.Add(checksItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChecksItem", new { id = checksItem.ID }, checksItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutChecksItem(Guid id, Checks checksItem)
        {
            if (id != checksItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(checksItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChecksItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChecksItem(Guid id)
        {
            var checksItem = await _context.Checks.FindAsync(id);
            if (checksItem == null)
            {
                return NotFound();
            }

            _context.Checks.Remove(checksItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChecksItemExists(Guid id)
        {
            return _context.Checks.Any(e => e.ID == id);
        }
    }
}
