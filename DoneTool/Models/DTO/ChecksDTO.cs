// <copyright file="ChecksDTO.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.DTO
{
    /// <summary>
    /// Data Transfer Object for the Checks entity.
    /// Used to transfer data related to Checks between the server and client.
    /// </summary>
    public class ChecksDTO
    {
        /// <summary>
        /// Gets or sets the name or description of the check item.
        /// </summary>
        public string Item { get; set; } = string.Empty;
    }
}
