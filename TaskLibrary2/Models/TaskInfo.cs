// <copyright file="TaskInfo.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TaskLibrary2.Models
{
    /// <summary>
    /// Represents the information about a task, including its ID, title, type, developer, and guard.
    /// </summary>
    public class TaskInfo
    {
        /// <summary>
        /// Gets or sets the unique identifier for the task.
        /// </summary>
        public int TaskID { get; set; }

        /// <summary>
        /// Gets or sets the title or name of the task.
        /// </summary>
        public string TaskTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type or category of the task.
        /// </summary>
        public string TaskType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the developer assigned to the task.
        /// </summary>
        public string Developer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the guard responsible for the task.
        /// </summary>
        public string Guard { get; set; } = string.Empty;
    }
}