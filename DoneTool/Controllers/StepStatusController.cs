namespace DoneTool.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using DoneTool.Data;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class StepStatusController : ControllerBase
    {
        private readonly DoneToolContext context;

        public StepStatusController(DoneToolContext context)
        {
            this.context = context;
        }

        // GET: api/StepStatus/IsStepDone/{stepId}
        [HttpGet("IsStepDone/{stepId}")]
        public async Task<ActionResult<bool>> IsStepDone(Guid stepId)
        {
            var step = await context.TaskChecklist.FindAsync(stepId);
            if (step == null)
            {
                return this.NotFound();
            }

            return this.Ok(step.Status == Models.Domain.TaskStatus.DONE);
        }

        // GET: api/StepStatus/AreAllStepsDone/{taskId}
        [HttpGet("AreAllStepsDone/{taskId}")]
        public async Task<ActionResult<bool>> AreAllStepsDone(int taskId)
        {
            var steps = this.context.TaskChecklist.Where(tc => tc.TaskID == taskId);
            if (!steps.Any())
            {
                return this.NotFound();
            }

            bool allDone = steps.All(step => step.Status == Models.Domain.TaskStatus.DONE);

            return this.Ok(allDone);
        }
    }
}