// <copyright file="DriverResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.SkylineApiModels
{
    /// <summary>
    /// Represents the response received from the Skyline API related to driver information.
    /// </summary>
    public class DriverResponse
    {
        /// <summary>
        /// Gets or sets the product owner information associated with the driver.
        /// </summary>
        /// <value>
        /// A <see cref="PersonReference"/> object that contains details about the product owner.
        /// </value>
        public PersonReference ProductOwner { get; set; } = new PersonReference();

        /// <summary>
        /// Gets or sets the list of code owners associated with the driver.
        /// </summary>
        /// <value>
        /// A list of <see cref="PersonReference"/> objects that represent the code owners.
        /// </value>
        public List<PersonReference> CodeOwner { get; set; } = new List<PersonReference>();

        /// <summary>
        /// Gets or sets the task creator information.
        /// </summary>
        /// <value>
        /// A <see cref="PersonReference"/> object that contains details about the creator.
        /// </value>
        public PersonReference Creator { get; set; } = new PersonReference();
    }
}
