// <copyright file="TaskViewModel.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.ViewModels
{
    /// <summary>
    /// Represents a view model for displaying detailed information about a specific task step.
    /// </summary>
    public class TaskViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the task checklist entry.
        /// </summary>
        /// <value>
        /// A <see cref="Guid"/> representing the ID of the task checklist entry.
        /// </value>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the title of the task step.
        /// </summary>
        /// <value>
        /// A string representing the title of the task step.
        /// </value>
        public string Step { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the selected status of the task step.
        /// </summary>
        /// <value>
        /// A string representing the current status of the task step (e.g., TODO, DONE).
        /// </value>
        public string SelectedStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the guard associated with the task step.
        /// </summary>
        /// <value>
        /// A string representing the name of the guard assigned to the task step.
        /// </value>
        public string Guard { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the comment related to the task step.
        /// </summary>
        /// <value>
        /// A string representing the comment added to the task step.
        /// </value>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the last update made to the task step.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> representing the date and time of the last update.
        /// </value>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the list of reasons for skipping the task step.
        /// </summary>
        /// <value>
        /// A list of strings representing the reasons for skipping the task step.
        /// </value>
        public List<string> SkipReasons { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the link associated with the task step.
        /// </summary>
        /// <value>
        /// A string representing a link associated with the task step.
        /// </value>
        public string Link { get; set; } = string.Empty;

        public Guid? OriginalTaskChecklistID { get; set; }

        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the list of duplicates for this task step.
        /// </summary>
        /// <value>
        /// A list of <see cref="TaskViewModel"/> representing the duplicates of the task step.
        /// </value>
        public List<TaskViewModel> Duplicates { get; set; } = new List<TaskViewModel>();
    }
}
