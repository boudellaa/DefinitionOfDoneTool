// <copyright file="PageModel.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.ViewModels
{
    /// <summary>
    /// Represents the view model for a task page, containing all necessary details for displaying task information.
    /// </summary>
    public class PageModel
    {
        /// <summary>
        /// Gets or sets the title of the task.
        /// </summary>
        /// <value>
        /// A string representing the task title.
        /// </value>
        public string TaskTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the developer associated with the task.
        /// </summary>
        /// <value>
        /// A string representing the developer's name.
        /// </value>
        public string DeveloperName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of checks associated with the task.
        /// </summary>
        /// <value>
        /// A list of <see cref="TaskViewModel"/> objects representing the checks.
        /// </value>
        public List<TaskViewModel> Checks { get; set; } = new List<TaskViewModel>();

        /// <summary>
        /// Gets or sets the list of guards available for the task.
        /// </summary>
        /// <value>
        /// A list of strings representing the names of available guards.
        /// </value>
        public List<string> Guards { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of suggested guards for the task.
        /// </summary>
        /// <value>
        /// A list of strings representing the names of suggested guards.
        /// </value>
        public List<string> SuggestedGuards { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the name of the Technical Account Manager (TAM) for the task.
        /// </summary>
        /// <value>
        /// A string representing the TAM's name.
        /// </value>
        public string TamName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the person who created the task.
        /// </summary>
        /// <value>
        /// A string representing the creator's name.
        /// </value>
        public string CreatorName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the Product Owner associated with the task.
        /// </summary>
        /// <value>
        /// A string representing the Product Owner's name.
        /// </value>
        public string ProductOwnerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of code owners' names associated with the task.
        /// </summary>
        /// <value>
        /// A list of strings representing the names of the code owners.
        /// </value>
        public List<string> CodeOwnerNames { get; set; } = new List<string>();
    }
}
