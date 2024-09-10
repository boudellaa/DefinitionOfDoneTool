// <copyright file="TaskViewModel.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Models.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class TaskViewModel
    {
        public Guid ID { get; set; }
        public string Step { get; set; }
        public string SelectedStatus { get; set; }
        public string Guard { get; set; }
        public string Comment { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
