// <copyright file="CheckSkipReason.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a reason for skipping a check in the task checklist.
    /// </summary>
    public class CheckSkipReason
    {
        /// <summary>
        /// Gets or sets the unique identifier for the CheckSkipReason.
        /// </summary>
        /// <value>
        /// A <see cref="Guid"/> that serves as the primary key.
        /// </value>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the check that this reason is associated with.
        /// </summary>
        /// <value>
        /// A <see cref="Guid"/> that represents the foreign key to the Checks table.
        /// </value>
        [ForeignKey("Checks")]
        public Guid CheckID { get; set; }

        /// <summary>
        /// Gets or sets the reason for skipping the check.
        /// </summary>
        /// <value>
        /// A string containing the reason for skipping the check, with a maximum length of 255 characters.
        /// </value>
        [Required]
        [StringLength(255)]
        public string Reason { get; set; } = string.Empty;
    }
}
