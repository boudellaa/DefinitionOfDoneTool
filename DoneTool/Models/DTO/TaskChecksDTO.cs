// <copyright file="TaskChecksDTO.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for the TaskChecks entity.
    /// Used to transfer data related to TaskChecks between the server and client.
    /// </summary>
    public class TaskChecksDTO
    {
        /// <summary>
        /// Gets or sets the step number associated with the task check.
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// Gets or sets the type of task associated with the task check.
        /// </summary>
        public string TaskType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the check associated with the task check.
        /// </summary>
        public Guid CheckID { get; set; }
    }
}
