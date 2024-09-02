// <copyright file="SQLTaskChecklistRepository.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Repositories.SQL
{
    using DoneTool.Data;
    using DoneTool.Models.Domain;
    using DoneTool.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// SQL Server implementation of the <see cref="ITaskChecklistRepository"/> interface for managing CRUD operations for TaskChecklists.
    /// This class utilizes a primary constructor to inject the database context.
    /// </summary>
    /// <param name="context">The database context used to interact with the database.</param>
    public class SQLTaskChecklistRepository(DoneToolContext context)
        : ITaskChecklistRepository
    {
        private readonly DoneToolContext context = context;

        /// <summary>
        /// Retrieves all task checklists from the database.
        /// </summary>
        /// <returns>A list of all task checklists.</returns>
        public async Task<List<TaskChecklist>> GetAllTaskChecklists()
        {
            return await this.context.TaskChecklist.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific task checklist by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task checklist.</param>
        /// <returns>The task checklist corresponding to the provided ID.</returns>
        public async Task<TaskChecklist> GetTaskChecklistById(Guid id)
        {
            var taskChecklist = await this.context.TaskChecklist.FirstOrDefaultAsync(tc => tc.ID == id);
            return taskChecklist ?? throw new InvalidOperationException($"Task checklist with ID {id} not found.");
        }

        /// <summary>
        /// Adds a new task checklist to the database.
        /// </summary>
        /// <param name="taskChecklist">The task checklist entity to be added.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task AddTaskChecklist(TaskChecklist taskChecklist)
        {
            await this.context.TaskChecklist.AddAsync(taskChecklist);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing task checklist in the database.
        /// </summary>
        /// <param name="taskChecklist">The task checklist entity with updated information.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task UpdateTaskChecklist(TaskChecklist taskChecklist)
        {
            this.context.TaskChecklist.Update(taskChecklist);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a task checklist from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the task checklist to be deleted.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task DeleteTaskChecklist(Guid id)
        {
            var taskChecklist = await this.context.TaskChecklist.FindAsync(id);
            if (taskChecklist != null)
            {
                this.context.TaskChecklist.Remove(taskChecklist);
                await this.context.SaveChangesAsync();
            }
        }
    }
}
