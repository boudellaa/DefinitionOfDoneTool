// <copyright file="SkylineUser.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.SkylineApiModels
{
    /// <summary>
    /// Represents a user in the Skyline system.
    /// </summary>
    public class SkylineUser
    {
        /// <summary>
        /// Gets or sets the name of the Skyline user.
        /// </summary>
        /// <value>
        /// A string representing the user's name. Defaults to an empty string.
        /// </value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the job title of the Skyline user.
        /// </summary>
        /// <value>
        /// A string representing the user's job title. Defaults to an empty string.
        /// </value>
        public string JobTitle { get; set; } = string.Empty;
    }
}
