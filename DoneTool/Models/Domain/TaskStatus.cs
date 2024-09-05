// <copyright file="TaskStatus.cs" company="Skyline Comunications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.Domain
{
    /// <summary>
    /// Represents the status of a task in the checklist.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// The task is yet to be done.
        /// </summary>
        TODO = 0,

        /// <summary>
        /// The task was skipped.
        /// </summary>
        SKIPPED = 1,

        /// <summary>
        /// The task is completed.
        /// </summary>
        DONE = 2,
    }
}
