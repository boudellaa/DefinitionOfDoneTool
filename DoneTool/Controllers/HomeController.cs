// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable CS8073

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

    /// <summary>
    /// The HomeController class manages the main operations of the DoneTool application,
    /// including loading task information, interacting with Skyline API, and generating the
    /// main page view.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly JsonTaskService jsonTaskService;
        private readonly TaskService taskService;
        private readonly DoneToolContext context;
        private readonly SkylineApiService skylineApiService;
        private readonly ILogger<HomeController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="jsonTaskService">Service for handling JSON tasks.</param>
        /// <param name="taskService">Service for handling task-related operations.</param>
        /// <param name="context">Database context for accessing task-related data.</param>
        /// <param name="skylineApiService">Service for interacting with the Skyline API.</param>
        /// <param name="logger">Logger for capturing log information.</param>
        public HomeController(
                              JsonTaskService jsonTaskService,
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

        /// <summary>
        /// Handles the HTTP GET request for the main page of the application.
        /// Loads task information from a JSON file, interacts with the Skyline API to retrieve
        /// additional task details, and prepares the data for rendering the page.
        /// </summary>
        /// <returns>The main page view with task and guard information.</returns>
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
            AddRole(info.CreatorName, "Task Creator");

            foreach (var codeOwnerName in info.CodeOwnerNames.Distinct())
            {
                AddRole(codeOwnerName, "DCO");
            }

            var suggestedGuards = rolesDict.ToHashSet();

            var usersJson = await this.skylineApiService.GetSkylineUsersAsync();
            var users = JsonConvert.DeserializeObject<List<SkylineUser>>(usersJson);

            if (users == null)
            {
                return this.NotFound("Skyline users could not be loaded.");
            }

            var otherGuards = new HashSet<string>(users.Select(u => u.Name).Except(rolesDict.Keys).ToList());
            var sortedGuards = otherGuards.OrderBy(g => g).ToList();

            var checksWithTaskChecklist = this.taskService.GetChecksForTask(taskInfo);

            var orderedChecks = this.context.TaskChecklist
                .Where(tc => tc.OriginalTaskChecklistID == null)
                .Select(tc => new TaskViewModel
                {
                    ID = tc.ID,

                    Step = this.context.TaskChecks
                            .Where(check => check.ID == tc.TaskChecksID)
                            .Select(check => this.context.Checks
                                           .Where(c => c.ID == check.CheckID)
                                           .Select(c => c.Item)
                                           .FirstOrDefault())
                            .FirstOrDefault() ?? string.Empty,

                    Link = this.context.TaskChecks
                            .Where(check => check.ID == tc.TaskChecksID)
                            .Select(check => this.context.Checks
                                             .Where(c => c.ID == check.CheckID)
                                             .Select(c => c.Link)
                                             .FirstOrDefault())
                            .FirstOrDefault() ?? string.Empty,

                    SelectedStatus = tc.Status.ToString(),
                    Comment = tc.Comment ?? string.Empty,
                    Guard = tc.Guard ?? string.Empty,
                    LastUpdated = tc.LastUpdated,

                    SkipReasons = this.context.TaskChecks
                        .Where(check => check.ID == tc.TaskChecksID)
                        .Select(check => this.context.Checks
                            .Where(c => c.ID == check.CheckID)
                            .Select(c => this.context.CheckSkipReasons
                                .Where(cs => cs.CheckID == c.ID)
                                .Select(cs => cs.Reason)
                                .ToList())
                            .FirstOrDefault())
                        .FirstOrDefault() ?? new List<string>(),

                    Duplicates = this.context.TaskChecklist
                        .Where(dup => dup.OriginalTaskChecklistID == tc.ID)
                        .Select(dup => new TaskViewModel
                        {
                            ID = dup.ID,
                            SelectedStatus = dup.Status.ToString(),
                            Comment = dup.Comment ?? string.Empty,
                            Guard = dup.Guard ?? string.Empty,
                            LastUpdated = dup.LastUpdated,
                            OriginalTaskChecklistID = dup.OriginalTaskChecklistID,
                        }).ToList(),
                })
                .OrderBy(task => this.context.TaskChecklist
                     .Where(tc => tc.ID == task.ID)
                     .Select(tc => this.context.TaskChecks
                                   .Where(tCheck => tCheck.ID == tc.TaskChecksID)
                                   .Select(tCheck => tCheck.Step)
                                   .FirstOrDefault())
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
                Checks = orderedChecks.Select(task =>
                {
                    var taskViewModels = new List<TaskViewModel>();

                    var actionType = string.Empty;

                    if (task.Step.Contains("Kickoff meeting"))
                    {
                        actionType = "Kickoff";
                    }
                    else if (task.Step.Contains("Code Review"))
                    {
                        actionType = "SendToCR";
                    }
                    else if (task.Step.Contains("Quality Assurance"))
                    {
                        actionType = "SendToQA";
                    }

                    taskViewModels.Add(new TaskViewModel
                    {
                        ID = task.ID,
                        Step = $"{stepNumber++.ToString("D2")}. {task.Step}",
                        SelectedStatus = task.SelectedStatus,
                        Comment = task.Comment,
                        Guard = task.Guard,
                        LastUpdated = task.LastUpdated,
                        OriginalTaskChecklistID = null,
                        SkipReasons = task.SkipReasons,
                        Link = task.Link ?? string.Empty,
                        ActionType = actionType ?? string.Empty,
                    });

                    foreach (var duplicate in task.Duplicates)
                    {
                        taskViewModels.Add(new TaskViewModel
                        {
                            ID = duplicate.ID,
                            Step = string.Empty,
                            SelectedStatus = duplicate.SelectedStatus,
                            Comment = duplicate.Comment,
                            Guard = duplicate.Guard,
                            LastUpdated = duplicate.LastUpdated,
                            OriginalTaskChecklistID = duplicate.OriginalTaskChecklistID,
                            SkipReasons = duplicate.SkipReasons,
                            Link = string.Empty,
                            ActionType = string.Empty,
                        });
                    }

                    return taskViewModels;
                }).SelectMany(tvm => tvm).ToList(),
            };

            return this.View(viewModel);
        }
    }
}