﻿// <copyright file="TaskChecklist.cs" company="Skyline Comunications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.Domain
{
    using System.Collections.Generic;
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

        public static implicit operator List<object>(TaskChecklist v)
        {
            throw new NotImplementedException();
        }
    }
}
