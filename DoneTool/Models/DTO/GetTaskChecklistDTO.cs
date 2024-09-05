// <copyright file="GetTaskChecklistDTO.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for retrieving a specific TaskChecklist.
    /// Inherits from BaseTaskChecklistDTO.
    /// </summary>
    public class GetTaskChecklistDTO : BaseTaskChecklistDTO
    {
        /// <summary>
        /// Gets or sets the guard assigned to the task checklist.
        /// </summary>
        public string Guard { get; set; } = string.Empty;
    }
}
