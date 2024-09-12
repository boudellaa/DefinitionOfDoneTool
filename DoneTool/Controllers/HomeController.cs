// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DoneTool.Data;
    using DoneTool.Models.SkylineApiModels;
    using DoneTool.Models.ViewModels;
    using DoneTool.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Skyline.DataMiner.Utils.JsonOps.Services;

    public class HomeController : Controller
    {
        private readonly JsonTaskService jsonTaskService;
        private readonly TaskService taskService;
        private readonly DoneToolContext context;
        private readonly SkylineApiService skylineApiService;
        private readonly ILogger<HomeController> logger;

        public HomeController(JsonTaskService jsonTaskService,
                              TaskService taskService,
                              DoneToolContext context,
                              SkylineApiService skylineApiService,
                              ILogger<HomeController> logger)
        {
            this.jsonTaskService = jsonTaskService;
            this.taskService = taskService;
            this.context = context;
            this.skylineApiService = skylineApiService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string jsonFilePath = "../JsonNuGeT/Utils.JsonOps/Data/task.json";
            var taskInfo = this.jsonTaskService.ReadTaskFromJson(jsonFilePath);

            if (taskInfo == null)
            {
                return this.NotFound("Task information could not be loaded from the JSON file.");
            }

            var info = await this.skylineApiService.GetTaskDetailsAsync(taskInfo);

            var suggestedGuards = new HashSet<string>
            {
                info.ProductOwnerName,
                info.TamName,
                info.CreatorName,
            };

            suggestedGuards.UnionWith(info.CodeOwnerNames.Distinct());

            var usersJson = await this.skylineApiService.GetSkylineUsersAsync();
            var users = JsonConvert.DeserializeObject<List<SkylineUser>>(usersJson);

            var otherGuards = users.Select(u => u.Name).Except(suggestedGuards).ToList();

            var allGuards = new HashSet<string>(otherGuards);

            var checksWithTaskChecklist = this.taskService.GetChecksForTask(taskInfo);
            int stepNumber = 1;

            string? tamName = info.TamName;
            string? creatorName = info.CreatorName;
            string? productOwnerName = info.ProductOwnerName;

            if (tamName == creatorName)
            {
                creatorName = null;
            }

            if (tamName == productOwnerName)
            {
                productOwnerName = null;
            }

            var viewModel = new PageModel
            {
                TaskTitle = info.Title,
                DeveloperName = info.AssigneeName,
                Guards = allGuards.ToList(),
                TamName = tamName,
                CreatorName = creatorName,
                ProductOwnerName = productOwnerName,
                CodeOwnerNames = info.CodeOwnerNames.Distinct().ToList(),
                Checks = checksWithTaskChecklist.Select(cwtc => new TaskViewModel
                {
                    ID = cwtc.TaskChecklistID,
                    Step = $"{stepNumber++.ToString("D2")}. {cwtc.Check.Item}",
                    SelectedStatus = this.context.TaskChecklist
                                               .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                               .Select(tc => tc.Status.ToString())
                                               .FirstOrDefault() ?? "TODO",
                    Comment = this.context.TaskChecklist
                                          .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                          .Select(tc => tc.Comment)
                                          .FirstOrDefault() ?? string.Empty,
                    Guard = string.Empty,
                    LastUpdated = this.context.TaskChecklist
                                   .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                   .Select(tc => tc.LastUpdated)
                                   .FirstOrDefault(),
                }).ToList(),
            };

            return this.View(viewModel);
        }

    }
}
