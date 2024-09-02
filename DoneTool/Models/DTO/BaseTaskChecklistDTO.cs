// <copyright file="BaseTaskChecklistDTO.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.DTO
{
    /// <summary>
    /// Base Data Transfer Object for TaskChecklist.
    /// Contains common properties shared by other DTOs related to TaskChecklist.
    /// </summary>
    public class BaseTaskChecklistDTO
    {
        /// <summary>
        /// Gets or sets the status of the task checklist.
        /// </summary>
        public Domain.TaskStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the comment associated with the task checklist.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the guard assigned to the task checklist.
        /// </summary>
        public string Guard { get; set; } = string.Empty;
    }
}
