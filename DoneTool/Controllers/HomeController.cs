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
        private readonly LinkGeneratorService linkGeneratorService;

        public HomeController(JsonTaskService jsonTaskService,
                              TaskService taskService,
                              DoneToolContext context,
                              SkylineApiService skylineApiService,
                              ILogger<HomeController> logger,
                              LinkGeneratorService linkGeneratorService)
        {
            this.jsonTaskService = jsonTaskService;
            this.taskService = taskService;
            this.context = context;
            this.skylineApiService = skylineApiService;
            this.logger = logger;
            this.linkGeneratorService = linkGeneratorService;
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

            var rolesDict = new Dictionary<string, List<string>>();

            void AddRole(string name, string role)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (!rolesDict.ContainsKey(name))
                    {
                        rolesDict[name] = new List<string>();
                    }

                    if (!rolesDict[name].Contains(role))
                    {
                        rolesDict[name].Add(role);
                    }
                }
            }

            AddRole(info.ProductOwnerName, "DPO");
            AddRole(info.TamName, "TAM");
            AddRole(info.CreatorName, "TC");

            foreach (var codeOwnerName in info.CodeOwnerNames.Distinct())
            {
                AddRole(codeOwnerName, "DCO");
            }

            var suggestedGuards = rolesDict.ToHashSet();

            var usersJson = await this.skylineApiService.GetSkylineUsersAsync();
            var users = JsonConvert.DeserializeObject<List<SkylineUser>>(usersJson);

            var otherGuards = new HashSet<string>(users.Select(u => u.Name).Except(rolesDict.Keys).ToList());
            var sortedGuards = otherGuards.OrderBy(g => g).ToList();

            var checksWithTaskChecklist = this.taskService.GetChecksForTask(taskInfo);

            var orderedChecks = checksWithTaskChecklist
                .OrderBy(cwtc => this.context.TaskChecks
                                            .Where(tch => tch.ID == this.context.TaskChecklist
                                                                    .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                                                    .Select(tc => tc.TaskChecksID)
                                                                    .FirstOrDefault())
                                            .Select(tch => tch.Step)
                                            .FirstOrDefault())
                .ThenBy(cwtc => this.context.TaskChecklist
                                            .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                            .Select(tc => tc.IsDuplicate)
                                            .FirstOrDefault())
                .ToList();

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

            var sortedRolesDict = rolesDict.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var sortedSuggestedGuards = sortedRolesDict
                                        .Select(kvp => $"{kvp.Key} ({string.Join(", ", kvp.Value)})")
                                        .ToList();

            var viewModel = new PageModel
            {
                TaskTitle = info.Title,
                DeveloperName = info.AssigneeName,
                SuggestedGuards = sortedSuggestedGuards,
                Guards = sortedGuards,
                TamName = rolesDict.ContainsKey(info.TamName) ?
                $"{info.TamName} ({string.Join(", ", rolesDict[info.TamName])})"
                : string.Empty,
                CreatorName = creatorName != null &&
                rolesDict.ContainsKey(info.CreatorName)
                ? $"{info.CreatorName} ({string.Join(", ", rolesDict[info.CreatorName])})"
                : string.Empty,
                ProductOwnerName = productOwnerName != null &&
                rolesDict.ContainsKey(info.ProductOwnerName)
                ? $"{info.ProductOwnerName} ({string.Join(", ", rolesDict[info.ProductOwnerName])})"
                : string.Empty,
                CodeOwnerNames = info.CodeOwnerNames.Distinct().Select(name =>
                {
                    if (rolesDict.ContainsKey(name))
                    {
                        var roles = string.Join(", ", rolesDict[name]);
                        return $"{name} ({roles})";
                    }

                    return name;
                }).ToList(),
                Checks = orderedChecks.Select(cwtc =>
                {
                    var isDuplicate = this.context.TaskChecklist
                                        .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                        .Select(tc => tc.IsDuplicate)
                                        .FirstOrDefault();

                    if (!isDuplicate)
                    {
                        return new TaskViewModel
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
                            Guard = this.context.TaskChecklist
                                                  .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                                  .Select(tc => tc.Guard)
                                                  .FirstOrDefault() ?? string.Empty,
                            LastUpdated = this.context.TaskChecklist
                                               .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                               .Select(tc => tc.LastUpdated)
                                               .FirstOrDefault(),
                            IsDuplicate = isDuplicate,
                            SkipReasons = this.context.CheckSkipReasons
                                                .Where(csr => csr.CheckID == cwtc.Check.ID)
                                                .Select(csr => csr.Reason)
                                                .ToList(),
                            Link = this.linkGeneratorService.GenerateStepLink(cwtc.Check.Item),
                        };
                    }
                    else
                    {
                        return new TaskViewModel
                        {
                            ID = cwtc.TaskChecklistID,
                            Step = $"{cwtc.Check.Item}",
                            SelectedStatus = this.context.TaskChecklist
                                                        .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                                        .Select(tc => tc.Status.ToString())
                                                        .FirstOrDefault() ?? "TODO",
                            Comment = this.context.TaskChecklist
                                                    .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                                    .Select(tc => tc.Comment)
                                                    .FirstOrDefault() ?? string.Empty,
                            Guard = this.context.TaskChecklist
                                                  .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                                  .Select(tc => tc.Guard)
                                                  .FirstOrDefault() ?? string.Empty,
                            LastUpdated = this.context.TaskChecklist
                                               .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                               .Select(tc => tc.LastUpdated)
                                               .FirstOrDefault(),
                            IsDuplicate = isDuplicate,
                            SkipReasons = this.context.CheckSkipReasons
                                                .Where(csr => csr.CheckID == cwtc.Check.ID)
                                                .Select(csr => csr.Reason)
                                                .ToList(),
                            Link = this.linkGeneratorService.GenerateStepLink(cwtc.Check.Item),
                        };
                    }
                }).ToList(),
            };

            return this.View(viewModel);
        }
    }
}
