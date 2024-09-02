// <copyright file="ITaskChecksRepository.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Repositories.Interfaces
{
    using DoneTool.Models.Domain;

    /// <summary>
    /// Interface for managing CRUD operations for TaskChecks.
    /// </summary>
    public interface ITaskChecksRepository
    {
        /// <summary>
        /// Retrieves all TaskChecks from the database.
        /// </summary>
        /// <returns>A list of all TaskChecks.</returns>
        Task<List<TaskChecks>> GetAllTaskChecks();

        /// <summary>
        /// Retrieves a specific TaskCheck by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the TaskCheck.</param>
        /// <returns>The TaskCheck corresponding to the provided ID.</returns>
        Task<TaskChecks> GetTaskCheckById(Guid id);

        /// <summary>
        /// Adds a new TaskCheck to the database.
        /// </summary>
        /// <param name="taskCheck">The TaskCheck entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddTaskCheck(TaskChecks taskCheck);

        /// <summary>
        /// Updates an existing TaskCheck in the database.
        /// </summary>
        /// <param name="taskCheck">The TaskCheck entity with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateTaskCheck(TaskChecks taskCheck);

        /// <summary>
        /// Deletes a TaskCheck from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the TaskCheck to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteTaskCheck(Guid id);
    }
}