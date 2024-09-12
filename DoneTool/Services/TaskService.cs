// <copyright file="TaskService.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DoneTool.Data;
    using DoneTool.Models.Domain;
    using DoneTool.Models.DTO;
    using DoneTool.Models.SkylineApiModels;
    using Microsoft.Extensions.Options;
    using Skyline.DataMiner.Utils.JsonOps.Models;

    /// <summary>
    /// Service class to manage task-related operations, such as retrieving checks for a task.
    /// </summary>
    public class TaskService
    {
        private readonly DoneToolContext context;
        private readonly SkylineApiService skylineApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="context">The database context used to interact with the DoneTool database.</param>
        public TaskService(DoneToolContext context, SkylineApiService skylineApiService)
        {
            this.context = context;
            this.skylineApiService = skylineApiService;
        }

        public List<CheckWithChecklistID> GetChecksForTask(TaskInfo taskInfo)
        {
            var taskChecklistEntry = this.context.TaskChecklist.FirstOrDefault(tc => tc.TaskID == taskInfo.TaskID);
            if (taskChecklistEntry == null)
            {
                return this.CreateNewRelationships(taskInfo);
            }

            return this.GetChecksForExistingTask(taskChecklistEntry);
        }

        private List<CheckWithChecklistID> GetChecksForExistingTask(TaskChecklist? taskChecklistEntry)
        {
            if (taskChecklistEntry == null)
            {
                throw new ArgumentNullException(nameof(taskChecklistEntry), "The task checklist entry cannot be null.");
            }

            var taskChecks = this.context.TaskChecklist
                .Where(tc => tc.TaskID == taskChecklistEntry.TaskID)
                .ToList();

            var checkWithChecklistID = taskChecks
                .Select(tc => new
                {
                    Check = this.context.Checks.FirstOrDefault(c => c.ID == this.context.TaskChecks.FirstOrDefault(tch => tch.ID == tc.TaskChecksID).CheckID),
                    TaskChecklistID = tc.ID,
                    Step = this.context.TaskChecks.FirstOrDefault(tch => tch.ID == tc.TaskChecksID).Step,
                })
                .Where(cwtc => cwtc.Check != null)
                .OrderBy(cwtc => cwtc.Step)  // Order by Step column
                .Select(cwtc => new CheckWithChecklistID
                {
                    Check = cwtc.Check,
                    TaskChecklistID = cwtc.TaskChecklistID
                })
                .ToList();

            return checkWithChecklistID;
        }


        private TaskChecklist CreateTaskChecklistEntry(TaskInfo taskInfo, TaskChecks taskCheck)
        {
            return new TaskChecklist
            {
                ID = Guid.NewGuid(),
                TaskID = taskInfo.TaskID,
                TaskChecksID = taskCheck.ID,
                Status = TaskStatus.TODO,
                Guard = string.Empty,
            };
        }


        private List<CheckWithChecklistID> CreateNewRelationships(TaskInfo taskInfo)
        {
            var taskDetails = skylineApiService.GetTaskDetailsAsync(taskInfo).GetAwaiter().GetResult();
            var taskType = taskDetails.Type;

            var taskChecks = this.context.TaskChecks
                .Where(tc => tc.TaskType == taskType)
                .OrderBy(tc => tc.Step)
                .ToList();

            List<CheckWithChecklistID> newRelationships = new List<CheckWithChecklistID>();

            foreach (var taskCheck in taskChecks)
            {
                var newTaskChecklistEntry = CreateTaskChecklistEntry(taskInfo, taskCheck);
                this.context.TaskChecklist.Add(newTaskChecklistEntry);

                var check = this.context.Checks.FirstOrDefault(c => c.ID == taskCheck.CheckID);
                if (check != null)
                {
                    newRelationships.Add(new CheckWithChecklistID
                    {
                        Check = check,
                        TaskChecklistID = newTaskChecklistEntry.ID,
                    });
                }

                if (!string.IsNullOrEmpty(taskDetails.ProductOwnerName))
                {
                    var duplicateTaskChecklistEntry = CreateTaskChecklistEntry(taskInfo, taskCheck);
                    this.context.TaskChecklist.Add(duplicateTaskChecklistEntry);

                    if (check != null)
                    {
                        newRelationships.Add(new CheckWithChecklistID
                        {
                            Check = check,
                            TaskChecklistID = duplicateTaskChecklistEntry.ID,
                        });
                    }
                }
            }

            this.context.SaveChanges();

            return newRelationships;
        }
    }
}