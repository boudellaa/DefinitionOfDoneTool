// <copyright file="AutoMapperProfiles.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Mappings
{
    using AutoMapper;
    using DoneTool.Models.Domain;
    using DoneTool.Models.DTO;

    /// <summary>
    /// Configures AutoMapper profiles for mapping domain models to DTOs and vice versa.
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperProfiles"/> class.
        /// </summary>
        public AutoMapperProfiles()
        {
            this.CreateMap<Checks, ChecksDTO>().ReverseMap();
            this.CreateMap<TaskChecklist, PostTaskChecklistDTO>().ReverseMap();
            this.CreateMap<TaskChecklist, GetTaskChecklistsDTO>().ReverseMap();
            this.CreateMap<TaskChecklist, GetTaskChecklistDTO>().ReverseMap();
            this.CreateMap<TaskChecklist, PutTaskChecklistDTO>().ReverseMap();
            this.CreateMap<TaskChecks, TaskChecksDTO>().ReverseMap();
        }
    }
}
