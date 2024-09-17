// <copyright file="TaskResponse.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.SkylineApiModels
{
    /// <summary>
    /// Represents the response data for a task retrieved from the Skyline API.
    /// </summary>
    public class TaskResponse
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
        /// Gets or sets the assignee information for the task.
        /// </summary>
        /// <value>
        /// A <see cref="PersonReference"/> object that contains details about the person assigned to the task.
        /// </value>
        public PersonReference Assignee { get; set; } = new PersonReference();

        /// <summary>
        /// Gets or sets the developer information associated with the task.
        /// </summary>
        /// <value>
        /// A <see cref="PersonReference"/> object that contains details about the developer working on the task.
        /// </value>
        public PersonReference Developer { get; set; } = new PersonReference();

        /// <summary>
        /// Gets or sets the project manager information for the Skyline project associated with the task.
        /// </summary>
        /// <value>
        /// A <see cref="PersonReference"/> object that contains details about the Skyline project manager.
        /// </value>
        public PersonReference ProjectsSkylinePM { get; set; } = new PersonReference();

        /// <summary>
        /// Gets or sets the integration ID related to the task.
        /// </summary>
        /// <value>
        /// A string representing the integration ID. Defaults to an empty string.
        /// </value>
        public string IntegrationID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Technical Account Manager (TAM) information for the Skyline project associated with the task.
        /// </summary>
        /// <value>
        /// A <see cref="PersonReference"/> object that contains details about the Skyline TAM.
        /// </value>
        public PersonReference ProjectsSkylineTam { get; set; } = new PersonReference();

        /// <summary>
        /// Gets or sets the creator information for the task.
        /// </summary>
        /// <value>
        /// A <see cref="PersonReference"/> object that contains details about the person who created the task.
        /// </value>
        public PersonReference Creator { get; set; } = new PersonReference();
    }
}
