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
        /// <param name="skylineApiService">The service used to interact with the Skyline API.</param>
        public TaskService(DoneToolContext context, SkylineApiService skylineApiService)
        {
            this.context = context;
            this.skylineApiService = skylineApiService;
        }

        /// <summary>
        /// Retrieves checks associated with a task based on the task information.
        /// </summary>
        /// <param name="taskInfo">The task information used to identify the task.</param>
        /// <returns>A list of <see cref="CheckWithChecklistID"/> objects representing the checks associated with the task.</returns>
        public List<CheckWithChecklistID> GetChecksForTask(TaskInfo taskInfo)
        {
            var taskChecklistEntry = this.context.TaskChecklist.FirstOrDefault(tc => tc.TaskID == taskInfo.TaskID);
            if (taskChecklistEntry == null)
            {
                return this.CreateNewRelationships(taskInfo);
            }

            return this.GetChecksForExistingTask(taskChecklistEntry);
        }

        /// <summary>
        /// Retrieves checks for an existing task checklist entry.
        /// </summary>
        /// <param name="taskChecklistEntry">The existing task checklist entry.</param>
        /// <returns>A list of <see cref="CheckWithChecklistID"/> objects representing the checks associated with the existing task.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="taskChecklistEntry"/> is null.</exception>
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
                .Select(tc =>
                {
                    var taskCheck = this.context.TaskChecks.FirstOrDefault(tch => tch.ID == tc.TaskChecksID);
                    var checkId = taskCheck != null ? taskCheck.CheckID : (Guid?)null;
                    var step = taskCheck != null ? taskCheck.Step : (int?)null;

                    return new
                    {
                        Check = checkId != null ? this.context.Checks.FirstOrDefault(c => c.ID == checkId.Value) : null,
                        TaskChecklistID = tc.ID,
                        Step = step,
                    };
                })
                .Where(cwtc => cwtc.Check != null)
                .OrderBy(cwtc => cwtc.Step)
                .Select(cwtc => new CheckWithChecklistID
                {
                    Check = cwtc.Check!,
                    TaskChecklistID = cwtc.TaskChecklistID,
                })
                .ToList();

            return checkWithChecklistID;
        }

        /// <summary>
        /// Creates a new task checklist entry based on the provided task information and task check.
        /// </summary>
        /// <param name="taskInfo">The task information used to create the checklist entry.</param>
        /// <param name="taskCheck">The task check associated with the checklist entry.</param>
        /// <returns>A new <see cref="TaskChecklist"/> object representing the created checklist entry.</returns>
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

        /// <summary>
        /// Creates new relationships for a task by generating task checklist entries and associating checks with them.
        /// </summary>
        /// <param name="taskInfo">The task information used to create new relationships.</param>
        /// <returns>A list of <see cref="CheckWithChecklistID"/> objects representing the newly created relationships.</returns>
        private List<CheckWithChecklistID> CreateNewRelationships(TaskInfo taskInfo)
        {
            var taskDetails = this.skylineApiService.GetTaskDetailsAsync(taskInfo).GetAwaiter().GetResult();
            var taskType = taskDetails.Type;

            var taskChecks = this.context.TaskChecks
                .Where(tc => tc.TaskType == taskType)
                .OrderBy(tc => tc.Step)
                .ToList();

            List<CheckWithChecklistID> newRelationships = new List<CheckWithChecklistID>();

            foreach (var taskCheck in taskChecks)
            {
                var newTaskChecklistEntry = this.CreateTaskChecklistEntry(taskInfo, taskCheck);
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
            }

            this.context.SaveChanges();

            return newRelationships;
        }
    }
}