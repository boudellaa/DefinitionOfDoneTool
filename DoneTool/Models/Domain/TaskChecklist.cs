// <copyright file="TaskChecklist.cs" company="Skyline Comunications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.Domain
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents an entry in the task checklist, linking tasks to specific checks.
    /// </summary>
    public class TaskChecklist
    {
        /// <summary>
        /// Gets or sets the unique identifier for the task checklist entry.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the task associated with this checklist entry.
        /// </summary>
        public int TaskID { get; set; }

        /// <summary>
        /// Gets or sets the status of the task.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the comment for the task checklist entry.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the guard responsible for this task.
        /// </summary>
        public string Guard { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier for the associated task checks.
        /// </summary>
        public Guid TaskChecksID { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the last update to this record.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> representing the UTC date and time when this record was last updated.
        /// Defaults to the current UTC date and time when the record is created.
        /// </value>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public Guid? OriginalTaskChecklistID { get; set; }
    }
}
