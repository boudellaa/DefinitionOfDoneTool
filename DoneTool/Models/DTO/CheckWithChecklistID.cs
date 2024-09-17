// <copyright file="CheckWithChecklistID.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.DTO
{
    using DoneTool.Models.Domain;

    /// <summary>
    /// Represents a relationship between a check and a task checklist.
    /// </summary>
    public class CheckWithChecklistID
    {
        /// <summary>
        /// Gets or sets the check associated with this task checklist.
        /// </summary>
        /// <value>
        /// An instance of the <see cref="Checks"/> class representing the specific check.
        /// </value>
        public Checks Check { get; set; } = new Checks();

        /// <summary>
        /// Gets or sets the unique identifier of the task checklist.
        /// </summary>
        /// <value>
        /// A <see cref="Guid"/> that uniquely identifies the task checklist associated with the check.
        /// </value>
        public Guid TaskChecklistID { get; set; }
    }
}
