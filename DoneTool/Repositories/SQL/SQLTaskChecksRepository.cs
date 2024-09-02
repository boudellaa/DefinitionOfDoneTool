// <copyright file="SQLTaskChecksRepository.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Repositories.SQL
{
    using DoneTool.Data;
    using DoneTool.Models.Domain;
    using DoneTool.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// SQL Server implementation of the <see cref="ITaskChecksRepository"/> interface for managing CRUD operations for TaskChecks.
    /// This class utilizes a primary constructor to inject the database context.
    /// </summary>
    /// <param name="context">The database context used to interact with the database.</param>
    public class SQLTaskChecksRepository(DoneToolContext context)
        : ITaskChecksRepository
    {
        private readonly DoneToolContext context = context;

        /// <summary>
        /// Retrieves all task checks from the database.
        /// </summary>
        /// <returns>A list of all task checks.</returns>
        public async Task<List<TaskChecks>> GetAllTaskChecks()
        {
            return await this.context.TaskChecks.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific task check by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task check.</param>
        /// <returns>The task check corresponding to the provided ID.</returns>
        public async Task<TaskChecks> GetTaskCheckById(Guid id)
        {
            var taskCheck = await this.context.TaskChecks.FirstOrDefaultAsync(tc => tc.ID == id);
            return taskCheck ?? throw new InvalidOperationException($"Task check with ID {id} not found.");
        }

        /// <summary>
        /// Adds a new task check to the database.
        /// </summary>
        /// <param name="taskCheck">The task check entity to be added.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task AddTaskCheck(TaskChecks taskCheck)
        {
            await this.context.TaskChecks.AddAsync(taskCheck);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing task check in the database.
        /// </summary>
        /// <param name="taskCheck">The task check entity with updated information.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task UpdateTaskCheck(TaskChecks taskCheck)
        {
            this.context.TaskChecks.Update(taskCheck);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a task check from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the task check to be deleted.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task DeleteTaskCheck(Guid id)
        {
            var taskCheck = await this.context.TaskChecks.FindAsync(id);
            if (taskCheck != null)
            {
                this.context.TaskChecks.Remove(taskCheck);
                await this.context.SaveChangesAsync();
            }
        }
    }
}