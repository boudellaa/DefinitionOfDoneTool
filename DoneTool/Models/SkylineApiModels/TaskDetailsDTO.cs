// <copyright file="TaskDetailsDTO.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.SkylineApiModels
{
    /// <summary>
    /// Represents detailed information about a task, including various related names and IDs.
    /// </summary>
    public class TaskDetailsDTO
    {
        /// <summary>
        /// Gets or sets the title of the task.
        /// </summary>
        /// <value>
        /// A string representing the task title. Defaults to an empty string.
        /// </value>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the task.
        /// </summary>
        /// <value>
        /// A string representing the task type. Defaults to an empty string.
        /// </value>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the person assigned to the task.
        /// </summary>
        /// <value>
        /// A string representing the assignee's name. Defaults to an empty string.
        /// </value>
        public string AssigneeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the developer associated with the task.
        /// </summary>
        /// <value>
        /// A string representing the developer's name. Defaults to an empty string.
        /// </value>
        public string DeveloperName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the integration ID related to the task.
        /// </summary>
        /// <value>
        /// A string representing the integration ID. Defaults to an empty string.
        /// </value>
        public string IntegrationID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the Technical Account Manager (TAM) for the task.
        /// </summary>
        /// <value>
        /// A string representing the TAM's name. Defaults to an empty string.
        /// </value>
        public string TamName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the person who created the task.
        /// </summary>
        /// <value>
        /// A string representing the creator's name. Defaults to an empty string.
        /// </value>
        public string CreatorName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the Product Owner associated with the task.
        /// </summary>
        /// <value>
        /// A string representing the Product Owner's name. Defaults to an empty string.
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
