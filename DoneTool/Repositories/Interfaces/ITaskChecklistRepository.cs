// <copyright file="ITaskChecklistRepository.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Repositories.Interfaces
{
    using DoneTool.Models.Domain;

    /// <summary>
    /// Interface for managing CRUD operations for TaskChecklist.
    /// </summary>
    public interface ITaskChecklistRepository
    {
        /// <summary>
        /// Retrieves all TaskChecklists from the database.
        /// </summary>
        /// <returns>A list of all TaskChecklists.</returns>
        Task<List<TaskChecklist>> GetAllTaskChecklists();

        /// <summary>
        /// Retrieves a specific TaskChecklist by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the TaskChecklist.</param>
        /// <returns>The TaskChecklist corresponding to the provided ID.</returns>
        Task<TaskChecklist> GetTaskChecklistById(Guid id);

        /// <summary>
        /// Adds a new TaskChecklist to the database.
        /// </summary>
        /// <param name="taskChecklist">The TaskChecklist entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddTaskChecklist(TaskChecklist taskChecklist);

        /// <summary>
        /// Updates an existing TaskChecklist in the database.
        /// </summary>
        /// <param name="taskChecklist">The TaskChecklist entity with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateTaskChecklist(TaskChecklist taskChecklist);

        /// <summary>
        /// Deletes a TaskChecklist from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the TaskChecklist to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteTaskChecklist(Guid id);
    }
}