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

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="context">The database context used to interact with the DoneTool database.</param>
        public TaskService(DoneToolContext context)
        {
            this.context = context;
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

            var checkWithChecklistID = taskChecks.Select(tc => new CheckWithChecklistID
            {
                Check = this.context.Checks.FirstOrDefault(c => c.ID == this.context.TaskChecks.FirstOrDefault(tch => tch.ID == tc.TaskChecksID).CheckID),
                TaskChecklistID = tc.ID,
            })
            .Where(cwtc => cwtc.Check != null)
            .ToList();

            return checkWithChecklistID;
        }

        private List<CheckWithChecklistID> CreateNewRelationships(TaskInfo taskInfo)
        {
            var taskChecks = this.context.TaskChecks
                .Where(tc => tc.TaskType == taskInfo.TaskType)
                .OrderBy(tc => tc.Step)
                .ToList();

            List<CheckWithChecklistID> newRelationships = new List<CheckWithChecklistID>();

            foreach (var taskCheck in taskChecks)
            {
                var newTaskChecklistEntry = new TaskChecklist
                {
                    ID = Guid.NewGuid(),
                    TaskID = taskInfo.TaskID,
                    TaskChecksID = taskCheck.ID,
                    Status = TaskStatus.TODO,
                    Guard = taskInfo.Guard,
                };

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