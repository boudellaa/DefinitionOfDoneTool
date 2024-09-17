// <copyright file="PersonReference.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.SkylineApiModels
{
    /// <summary>
    /// Represents a reference to a person, including their name, ID, and a reference string.
    /// </summary>
    public class PersonReference
    {
        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        /// <value>
        /// A string representing the person's name.
        /// </value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the person.
        /// </summary>
        /// <value>
        /// A string representing the person's unique ID.
        /// </value>
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the reference string associated with the person.
        /// </summary>
        /// <value>
        /// A string representing a reference that is related to the person.
        /// </value>
        public string Ref { get; set; } = string.Empty;
    }
}
