// <copyright file="TaskChecks.cs" company="Skyline Comunications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.Domain
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a task check which is a specific step or task in a checklist.
    /// </summary>
    public class TaskChecks
    {
        /// <summary>
        /// Gets or sets the unique identifier for the task check.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the step number within the task.
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// Gets or sets the type of the task.
        /// </summary>
        public string TaskType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier for the associated check.
        /// </summary>
        public Guid CheckID { get; set; }
    }
}
