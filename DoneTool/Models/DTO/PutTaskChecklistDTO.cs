// <copyright file="PutTaskChecklistDTO.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for updating an existing TaskChecklist.
    /// Inherits from BaseTaskChecklistDTO.
    /// </summary>
    public class PutTaskChecklistDTO : BaseTaskChecklistDTO
    {
        /// <summary>
        /// Gets or sets the timestamp of the last update to this record.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the guard assigned to the task checklist.
        /// </summary>
        public string Guard { get; set; } = string.Empty;
    }
}
