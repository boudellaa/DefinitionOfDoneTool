// <copyright file="IChecksRepository.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Repositories.Interfaces
{
    using DoneTool.Models.Domain;

    /// <summary>
    /// Interface for managing CRUD operations for Checks.
    /// </summary>
    public interface IChecksRepository
    {
        /// <summary>
        /// Retrieves all checks from the database.
        /// </summary>
        /// <returns>A list of all checks.</returns>
        Task<List<Checks>> GetAllChecks();

        /// <summary>
        /// Retrieves a specific check by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the check.</param>
        /// <returns>The check corresponding to the provided ID.</returns>
        Task<Checks> GetCheckById(Guid id);

        /// <summary>
        /// Adds a new check to the database.
        /// </summary>
        /// <param name="check">The check entity to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddCheck(Checks check);

        /// <summary>
        /// Updates an existing check in the database.
        /// </summary>
        /// <param name="check">The check entity with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateCheck(Checks check);

        /// <summary>
        /// Deletes a check from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the check to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteCheck(Guid id);
    }
}
