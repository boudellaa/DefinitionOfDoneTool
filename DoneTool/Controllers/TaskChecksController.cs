// <copyright file="TaskChecksController.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Controllers
{
    using AutoMapper;
    using DoneTool.Models.Domain;
    using DoneTool.Models.DTO;
    using DoneTool.Repositories.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskChecksController"/> class.
    /// </summary>
    /// <param name="taskChecksRepository">The repository to manage task checks.</param>
    /// <param name="mapper">The mapper for mapping between domain models and DTOs.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class TaskChecksController(ITaskChecksRepository taskChecksRepository, IMapper mapper)
        : ControllerBase
    {
        /// <summary>
        /// Gets all task checks.
        /// </summary>
        /// <returns>A list of <see cref="TaskChecksDTO"/> representing all task checks.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskChecksDTO>>> GetTaskChecks()
        {
            var taskChecks = await taskChecksRepository.GetAllTaskChecks();
            var taskChecksDTO = mapper.Map<IEnumerable<TaskChecksDTO>>(taskChecks);
            return this.Ok(taskChecksDTO);
        }

        /// <summary>
        /// Gets a specific task check by ID.
        /// </summary>
        /// <param name="id">The ID of the task check.</param>
        /// <returns>The <see cref="TaskChecksDTO"/> representing the requested task check.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskChecksDTO>> GetTaskCheck(Guid id)
        {
            var taskCheck = await taskChecksRepository.GetTaskCheckById(id);

            if (taskCheck == null)
            {
                return this.NotFound();
            }

            var taskCheckDTO = mapper.Map<TaskChecksDTO>(taskCheck);
            return this.Ok(taskCheckDTO);
        }

        /// <summary>
        /// Creates a new task check.
        /// </summary>
        /// <param name="taskCheckDTO">The DTO containing the details of the task check to create.</param>
        /// <returns>The created <see cref="TaskChecksDTO"/> with its ID.</returns>
        [HttpPost]
        public async Task<ActionResult<TaskChecksDTO>> PostTaskCheck(TaskChecksDTO taskCheckDTO)
        {
            var taskCheck = mapper.Map<TaskChecks>(taskCheckDTO);
            await taskChecksRepository.AddTaskCheck(taskCheck);

            var createdTaskCheckDTO = mapper.Map<TaskChecksDTO>(taskCheck);
            return this.CreatedAtAction(nameof(this.GetTaskCheck), new { id = taskCheck.ID }, createdTaskCheckDTO);
        }

        /// <summary>
        /// Updates an existing task check.
        /// </summary>
        /// <param name="id">The ID of the task check to update.</param>
        /// <param name="taskCheckDTO">The DTO containing the updated details of the task check.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskCheck(Guid id, TaskChecksDTO taskCheckDTO)
        {
            var existingTaskCheck = await taskChecksRepository.GetTaskCheckById(id);
            if (existingTaskCheck == null)
            {
                return this.NotFound();
            }

            existingTaskCheck.Step = taskCheckDTO.Step;
            existingTaskCheck.TaskType = taskCheckDTO.TaskType;
            existingTaskCheck.CheckID = taskCheckDTO.CheckID;

            await taskChecksRepository.UpdateTaskCheck(existingTaskCheck);

            return this.NoContent();
        }

        /// <summary>
        /// Deletes a task check.
        /// </summary>
        /// <param name="id">The ID of the task check to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskCheck(Guid id)
        {
            var taskCheck = await taskChecksRepository.GetTaskCheckById(id);
            if (taskCheck == null)
            {
                return this.NotFound();
            }

            await taskChecksRepository.DeleteTaskCheck(id);

            return this.NoContent();
        }
    }
}
