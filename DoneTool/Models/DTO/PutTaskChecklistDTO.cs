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
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the guard assigned to the task checklist.
        /// </summary>
        public string Guard { get; set; } = string.Empty;
    }
}
