// <copyright file="Checks.cs" company="Skyline Comunications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.Domain
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a check in the DoneTool system.
    /// </summary>
    public class Checks
    {
        /// <summary>
        /// Gets or sets the unique identifier for the check.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the name or description of the check.
        /// </summary>
        public string Item { get; set; } = string.Empty;
    }
}
