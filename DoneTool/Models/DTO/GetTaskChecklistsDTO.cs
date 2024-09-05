// <copyright file="GetTaskChecklistsDTO.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for retrieving a collection of TaskChecklists.
    /// Inherits from BaseTaskChecklistDTO.
    /// </summary>
    public class GetTaskChecklistsDTO : BaseTaskChecklistDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the task checklist.
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the guard assigned to the task checklist.
        /// </summary>
        public string Guard { get; set; } = string.Empty;
    }
}
