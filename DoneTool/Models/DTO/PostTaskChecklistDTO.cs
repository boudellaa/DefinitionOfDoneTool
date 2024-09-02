// <copyright file="PostTaskChecklistDTO.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for creating a new TaskChecklist.
    /// Inherits from BaseTaskChecklistDTO.
    /// </summary>
    public class PostTaskChecklistDTO : BaseTaskChecklistDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the task associated with the checklist.
        /// </summary>
        public int TaskID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the TaskChecks associated with the checklist.
        /// </summary>
        public Guid TaskChecksID { get; set; }
    }
}
