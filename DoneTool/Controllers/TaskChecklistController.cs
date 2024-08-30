using DoneTool.Data;
using DoneTool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoneTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskChecklistController : ControllerBase
    {
        private readonly DoneToolContext _context;

        public TaskChecklistController(DoneToolContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskChecklist>>> GetTaskChecklists()
        {
            return await _context.TaskChecklist.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TaskChecklist>> GetTaskChecklist(Guid id)
        {
            var taskChecklist = await _context.TaskChecklist.FindAsync(id);

            if (taskChecklist == null)
            {
                return NotFound();
            }

            return taskChecklist;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskChecklist(Guid id, TaskChecklist taskChecklist)
        {
            if (id != taskChecklist.ID)
            {
                return BadRequest();
            }

            _context.Entry(taskChecklist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskChecklistExists(id))
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
        public async Task<ActionResult<TaskChecklist>> PostTaskChecklist(TaskChecklist taskChecklist)
        {
            _context.TaskChecklist.Add(taskChecklist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskChecklist", new { id = taskChecklist.ID }, taskChecklist);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskChecklist(Guid id)
        {
            var taskChecklist = await _context.TaskChecklist.FindAsync(id);
            if (taskChecklist == null)
            {
                return NotFound();
            }

            _context.TaskChecklist.Remove(taskChecklist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskChecklistExists(Guid id)
        {
            return _context.TaskChecklist.Any(e => e.ID == id);
        }
    }
}
