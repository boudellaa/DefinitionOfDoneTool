using DoneTool.Data;
using DoneTool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoneTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskChecksController : ControllerBase
    {
        private readonly DoneToolContext _context;

        public TaskChecksController(DoneToolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskChecks>>> GetTaskChecks()
        {
            return await _context.TaskChecks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskChecks>> GetTaskCheck(Guid id)
        {
            var taskCheck = await _context.TaskChecks.FindAsync(id);

            if (taskCheck == null)
            {
                return NotFound();
            }

            return taskCheck;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskCheck(Guid id, TaskChecks taskCheck)
        {
            if (id != taskCheck.ID)
            {
                return BadRequest();
            }

            _context.Entry(taskCheck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskCheckExists(id))
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

        [HttpPost]
        public async Task<ActionResult<TaskChecks>> PostTaskCheck(TaskChecks taskCheck)
        {
            _context.TaskChecks.Add(taskCheck);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTempTable", new { id = taskCheck.ID }, taskCheck);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskCheck(Guid id)
        {
            var taskCheck = await _context.TaskChecks.FindAsync(id);
            if (taskCheck == null)
            {
                return NotFound();
            }

            _context.TaskChecks.Remove(taskCheck);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskCheckExists(Guid id)
        {
            return _context.TaskChecks.Any(e => e.ID == id);
        }
    }
}
