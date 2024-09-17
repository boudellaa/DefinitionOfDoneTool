// <copyright file="SkylineApiData.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.SkylineApiModels
{
    /// <summary>
    /// Represents the data required for authenticating with the Skyline API.
    /// </summary>
    public class SkylineApiData
    {
        /// <summary>
        /// Gets or sets the username used for Skyline API authentication.
        /// </summary>
        /// <value>
        /// A string representing the username. Defaults to an empty string.
        /// </value>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password used for Skyline API authentication.
        /// </summary>
        /// <value>
        /// A string representing the password. Defaults to an empty string.
        /// </value>
        public string Password { get; set; } = string.Empty;
    }
}
