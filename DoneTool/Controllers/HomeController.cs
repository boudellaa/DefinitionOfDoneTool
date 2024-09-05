// <copyright file="HomeController.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Controllers
{
    using DoneTool.Data;
    using DoneTool.Models.ViewModels;
    using DoneTool.Services;
    using Microsoft.AspNetCore.Mvc;
    using Skyline.DataMiner.Utils.JsonOps.Services;

    public class HomeController : Controller
    {
        private readonly JsonTaskService jsonTaskService;
        private readonly TaskService taskService;
        private readonly DoneToolContext context;

        public HomeController(JsonTaskService jsonTaskService, TaskService taskService, DoneToolContext context)
        {
            this.jsonTaskService = jsonTaskService;
            this.taskService = taskService;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string jsonFilePath = "../JsonNuGeT/Utils.JsonOps/Data/task.json";

            var taskInfo = this.jsonTaskService.ReadTaskFromJson(jsonFilePath);

            if (taskInfo == null)
            {
                return this.NotFound("Task information could not be loaded from the JSON file.");
            }

            var checksWithTaskChecklist = this.taskService.GetChecksForTask(taskInfo);

            var viewModel = new PageModel
            {
                TaskTitle = taskInfo.TaskTitle,
                DeveloperName = taskInfo.Developer,
                Checks = checksWithTaskChecklist.Select(cwtc => new TaskViewModel
                {
                    ID = cwtc.TaskChecklistID,
                    Subtask = cwtc.Check.Item,
                    SelectedStatus = this.context.TaskChecklist
                                       .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                       .Select(tc => tc.Status.ToString())
                                       .FirstOrDefault() ?? "TODO",
                    Comment = this.context.TaskChecklist
                                  .Where(tc => tc.ID == cwtc.TaskChecklistID)
                                  .Select(tc => tc.Comment)
                                  .FirstOrDefault() ?? string.Empty,
                    Guard = taskInfo.Guard,
                }).ToList(),
            };

            return this.View(viewModel);
        }
    }
}