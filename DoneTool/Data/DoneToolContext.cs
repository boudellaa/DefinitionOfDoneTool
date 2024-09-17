// <copyright file="DoneToolContext.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Data
{
    using DoneTool.Models.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoneToolContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public class DoneToolContext(DbContextOptions<DoneToolContext> options)
        : DbContext(options)
    {
        /// <summary>
        /// Gets or sets the DbSet of Checks.
        /// </summary>
        public DbSet<Checks> Checks { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of TaskChecklists.
        /// </summary>
        public DbSet<TaskChecklist> TaskChecklist { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of TaskChecks.
        /// </summary>
        public DbSet<TaskChecks> TaskChecks { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of CheckSkipReasons.
        /// </summary>
        /// <value>
        /// A collection of <see cref="CheckSkipReason"/> entities that represent reasons for skipping checks.
        /// </value>
        public DbSet<CheckSkipReason> CheckSkipReasons { get; set; }
    }
}
