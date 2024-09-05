// <copyright file="JsonTaskService.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Skyline.DataMiner.Utils.JsonOps.Services
{
    using System;
    using System.IO;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Skyline.DataMiner.Utils.JsonOps.Models;

    /// <summary>
    /// Provides methods to read and write task information to and from JSON files.
    /// </summary>
    public class JsonTaskService
    {
        private readonly ILogger<JsonTaskService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonTaskService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used to log messages and errors.</param>
        public JsonTaskService(ILogger<JsonTaskService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Reads the task information from a JSON file.
        /// </summary>
        /// <param name="filePath">The path to the JSON file containing the task information.</param>
        /// <returns>The deserialized <see cref="TaskInfo"/> object from the JSON file, or <c>null</c> if deserialization fails.</returns>
        public TaskInfo ReadTaskFromJson(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(jsonString))
            {
                this.logger.LogError($"The file at {filePath} is empty or could not be read.");
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<TaskInfo>(jsonString);
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, $"Failed to deserialize JSON from file at {filePath}. The file content might be malformed.");
                return null;
            }
        }

        /// <summary>
        /// Writes the task information to a JSON file.
        /// </summary>
        /// <param name="taskInfo">The <see cref="TaskInfo"/> object containing the task information to be serialized.</param>
        /// <param name="filePath">The path to the JSON file where the task information will be saved.</param>
        public void WriteTaskToJson(TaskInfo taskInfo, string filePath)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(taskInfo, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to write JSON to file at {filePath}. Please check if the file path is correct and accessible.");
            }
        }
    }
}