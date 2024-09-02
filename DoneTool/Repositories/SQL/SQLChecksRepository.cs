// <copyright file="SQLChecksRepository.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Repositories.SQL
{
    using DoneTool.Data;
    using DoneTool.Models.Domain;
    using DoneTool.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// SQL Server implementation of the <see cref="IChecksRepository"/> interface for managing CRUD operations for Checks.
    /// This class utilizes a primary constructor to inject the database context.
    /// </summary>
    /// <param name="context">The database context used to interact with the database.</param>
    public class SQLChecksRepository(DoneToolContext context)
        : IChecksRepository
    {
        private readonly DoneToolContext context = context;

        /// <summary>
        /// Retrieves all checks from the database.
        /// </summary>
        /// <returns>A list of all checks.</returns>
        public async Task<List<Checks>> GetAllChecks()
        {
            return await this.context.Checks.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific check by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the check.</param>
        /// <returns>The check corresponding to the provided ID.</returns>
        public async Task<Checks> GetCheckById(Guid id)
        {
            var check = await this.context.Checks.FirstOrDefaultAsync(c => c.ID == id);
            return check ?? throw new InvalidOperationException($"Check with ID {id} not found.");
        }

        /// <summary>
        /// Adds a new check to the database.
        /// </summary>
        /// <param name="check">The check entity to be added.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task AddCheck(Checks check)
        {
            await this.context.Checks.AddAsync(check);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing check in the database.
        /// </summary>
        /// <param name="check">The check entity with updated information.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task UpdateCheck(Checks check)
        {
            this.context.Checks.Update(check);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a check from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the check to be deleted.</param>
        /// <returns>A task representing the operation.</returns>
        public async Task DeleteCheck(Guid id)
        {
            var check = await this.context.Checks.FindAsync(id);
            if (check != null)
            {
                this.context.Checks.Remove(check);
                await this.context.SaveChangesAsync();
            }
        }
    }
}