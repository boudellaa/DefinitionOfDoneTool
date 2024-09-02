// <copyright file="TaskService.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Skyline.DataMiner.TaskDataMiner.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DoneTool.Data;
    using DoneTool.Models.Domain;
    using Skyline.DataMiner.TaskDataMiner.Models;

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

        /// <summary>
        /// Retrieves the list of checks associated with a task.
        /// </summary>
        /// <param name="taskInfo">The task information object containing details about the task.</param>
        /// <returns>A list of <see cref="Checks"/> associated with the task.</returns>
        public List<Checks> GetChecksForTask(TaskInfo taskInfo)
        {
            var taskChecklistEntry = this.context.TaskChecklist.FirstOrDefault(tc => tc.TaskID == taskInfo.TaskID);
            if (taskChecklistEntry == null)
            {
                return this.CreateNewRelationships(taskInfo);
            }

            return this.GetChecksForExistingTask(taskChecklistEntry);
        }

        /// <summary>
        /// Retrieves the checks for an existing task based on its checklist entry.
        /// </summary>
        /// <param name="taskChecklistEntry">The task checklist entry corresponding to the task.</param>
        /// <returns>A list of <see cref="Checks"/> associated with the existing task.</returns>
        private List<Checks> GetChecksForExistingTask(TaskChecklist taskChecklistEntry)
        {
            if (taskChecklistEntry == null)
            {
                throw new ArgumentNullException(nameof(taskChecklistEntry), "The task checklist entry cannot be null.");
            }

            var taskChecksIDs = this.context.TaskChecklist
                .Where(tc => tc.TaskID == taskChecklistEntry.TaskID)
                .Select(tc => tc.TaskChecksID)
                .ToList();

            var checkIDs = taskChecksIDs.Select(id => this.context.TaskChecks
                .Where(c => c.ID == id)
                .Select(c => c.CheckID)
                .FirstOrDefault())
                .Where(checkID => checkID != Guid.Empty)
                .ToList();

            return this.context.Checks.Where(c => checkIDs.Contains(c.ID)).ToList();
        }

        /// <summary>
        /// Creates new relationships between the task and its checks, and returns the associated checks.
        /// </summary>
        /// <param name="taskInfo">The task information object containing details about the task.</param>
        /// <returns>A list of <see cref="Checks"/> associated with the newly created task relationships.</returns>
        private List<Checks> CreateNewRelationships(TaskInfo taskInfo)
        {
            var taskChecksIDs = this.context.TaskChecks
                .Where(tc => tc.TaskType == taskInfo.TaskType)
                .Select(tc => tc.ID)
                .ToList();
            foreach (var taskChecksID in taskChecksIDs)
            {
                var newTaskChecklistEntry = new TaskChecklist
                {
                    ID = Guid.NewGuid(),
                    TaskID = taskInfo.TaskID,
                    TaskChecksID = taskChecksID,
                    Status = TaskStatus.TODO,
                    Guard = taskInfo.Guard,
                };

                this.context.TaskChecklist.Add(newTaskChecklistEntry);
            }

            this.context.SaveChanges();

            return this.GetChecksForExistingTask(this.context.TaskChecklist.FirstOrDefault(tc => tc.TaskID == taskInfo.TaskID));
        }
    }
}