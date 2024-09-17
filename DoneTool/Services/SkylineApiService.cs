// <copyright file="SkylineApiService.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using DoneTool.Models.SkylineApiModels;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Skyline.DataMiner.Utils.JsonOps.Models;

    /// <summary>
    /// Provides methods to interact with the Skyline API, including authentication and data retrieval.
    /// </summary>
    public class SkylineApiService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<SkylineApiService> logger;
        private readonly SkylineApiData options;
        private string accessToken = string.Empty;
        private string refreshToken = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkylineApiService"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send requests to the Skyline API.</param>
        /// <param name="logger">The logger used to log information, warnings, and errors.</param>
        /// <param name="options">The Skyline API options containing authentication data.</param>
        public SkylineApiService(HttpClient httpClient, ILogger<SkylineApiService> logger, IOptions<SkylineApiData> options)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.options = options.Value;
        }

        /// <summary>
        /// Retrieves Skyline users from the API.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response content as a string.</returns>
        public async Task<string> GetSkylineUsersAsync()
        {
            if (string.IsNullOrEmpty(this.accessToken))
            {
                this.accessToken = await this.GetAccessTokenAsync();
            }

            try
            {
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);

                var response = await this.httpClient.GetAsync("api/dcp/Users/Skyline");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                this.logger.LogInformation("Successfully fetched data from Skyline API.");
                return content;
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError("Error fetching data from Skyline API: {message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves detailed information about a task from the Skyline API.
        /// </summary>
        /// <param name="taskInfo">The task information object containing the task ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the task details as a <see cref="TaskDetailsDTO"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="taskInfo"/>.</exception>
        public async Task<TaskDetailsDTO> GetTaskDetailsAsync(TaskInfo taskInfo)
        {
            if (taskInfo == null || taskInfo.TaskID < 100000 || taskInfo.TaskID > 999999)
            {
                throw new ArgumentNullException(nameof(taskInfo), "TaskInfo or TaskID cannot be null or zero.");
            }

            try
            {
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);

                var taskData = await this.FetchTaskDataAsync(taskInfo.TaskID);

                var allPersonsTaskApi = new List<PersonReference>
                {
                    taskData.Assignee,
                    taskData.ProjectsSkylinePM,
                    taskData.ProjectsSkylineTam,
                    taskData.Developer,
                    taskData.Creator,
                };

                var assigneeName = ResolveName(taskData.Assignee, allPersonsTaskApi);
                var developerName = ResolveName(taskData.Developer, allPersonsTaskApi);
                var tamName = ResolveName(taskData.ProjectsSkylineTam, allPersonsTaskApi);
                var creatorName = ResolveName(taskData.Creator, allPersonsTaskApi);

                string productOwnerName = string.Empty;
                List<string> codeOwnerNames = new List<string>();

                if (string.IsNullOrEmpty(this.accessToken))
                {
                    this.accessToken = await this.GetAccessTokenAsync();
                }

                if (!string.IsNullOrEmpty(taskData.IntegrationID))
                {
                    var ownerResponse = await this.httpClient.GetAsync($"https://api.skyline.be/api/dcp/Drivers/ByIntegrationID?ids[0]={taskData.IntegrationID}");
                    ownerResponse.EnsureSuccessStatusCode();

                    var ownerContent = await ownerResponse.Content.ReadAsStringAsync();
                    var driverData = JsonConvert.DeserializeObject<List<DriverResponse>>(ownerContent);

                    if (driverData == null || !driverData.Any())
                    {
                        throw new Exception("No driver data returned from API.");
                    }

                    var driver = driverData.FirstOrDefault() ?? throw new Exception("No driver data returned from API.");
                    var allPersonsDriverApi = new List<PersonReference>()
                    {
                        driver.ProductOwner,
                        driver.Creator,
                    };

                    allPersonsDriverApi.AddRange(driver.CodeOwner);

                    if (driverData != null && driverData.Count != 0)
                    {
                        productOwnerName = ResolveName(driver.ProductOwner, allPersonsDriverApi);
                        codeOwnerNames.AddRange(driver.CodeOwner.Select(co => ResolveName(co, allPersonsDriverApi)));
                    }
                }

                return new TaskDetailsDTO
                {
                    Title = taskData.Title,
                    Type = taskData.Type,
                    AssigneeName = assigneeName,
                    DeveloperName = developerName,
                    IntegrationID = taskData.IntegrationID,
                    TamName = tamName,
                    CreatorName = creatorName,
                    ProductOwnerName = productOwnerName,
                    CodeOwnerNames = codeOwnerNames,
                };
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError("Error fetching task details from Skyline API: {message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Fetches task data from the Skyline API based on the task ID.
        /// </summary>
        /// <param name="taskId">The task ID used to retrieve the task data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the task data as a <see cref="TaskResponse"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if the task ID is not a valid six-digit number.</exception>
        public async Task<TaskResponse> FetchTaskDataAsync(int taskId)
        {
            if (taskId < 100000 || taskId > 999999)
            {
                throw new ArgumentException("TaskID must be a six-digit number.", nameof(taskId));
            }

            if (string.IsNullOrEmpty(this.accessToken))
            {
                this.accessToken = await this.GetAccessTokenAsync();
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            var response = await this.httpClient.GetAsync($"https://api.skyline.be/api/dcp/Tasks/ById?ids[0]={taskId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var taskDataList = JsonConvert.DeserializeObject<List<TaskResponse>>(content);

            if (taskDataList == null || !taskDataList.Any())
            {
                throw new Exception("No task data returned from API.");
            }

            return taskDataList.FirstOrDefault() ?? throw new Exception("No task data returned from API.");
        }

        /// <summary>
        /// Resolves the name of a person based on a reference and a list of all persons.
        /// </summary>
        /// <param name="reference">The person reference containing the ID or name.</param>
        /// <param name="allPersons">The list of all persons from which to resolve the name.</param>
        /// <returns>
        /// A string representing the resolved name of the person, or an empty string if the name could not be resolved.
        /// </returns>
        private static string ResolveName(PersonReference reference, List<PersonReference> allPersons)
        {
            if (reference == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(reference.Name))
            {
                return reference.Name;
            }

            if (reference.Ref == null)
            {
                return string.Empty;
            }

            var person = allPersons.FirstOrDefault(p => p.ID == reference?.Ref);
            return person?.Name ?? string.Empty;
        }

        /// <summary>
        /// Retrieves an access token for the Skyline API using the username and password.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the access token as a string.</returns>
        private async Task<string> GetAccessTokenAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "Token");

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", this.options.Username),
                new KeyValuePair<string, string>("password", this.options.Password),
                new KeyValuePair<string, string>("grant_type", "password"),
            });

            request.Content = content;
            var response = await this.httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadAsStringAsync();

            var tokenData = JsonConvert.DeserializeObject<TokenResponse>(tokenResponse);

            if (tokenData == null)
            {
                throw new Exception("No token data returned from API.");
            }

            this.accessToken = tokenData.AccessToken;
            this.refreshToken = tokenData.RefreshToken;

            this.logger.LogInformation("Access token acquired: {accessToken}", this.accessToken);

            return this.accessToken;
        }

        /// <summary>
        /// Represents the response structure for an authentication token.
        /// </summary>
        private class TokenResponse
        {
            /// <summary>
            /// Gets or sets the access token.
            /// </summary>
            [JsonProperty("access_token")]
            public string AccessToken { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the refresh token.
            /// </summary>
            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the token expiration time in seconds.
            /// </summary>
            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; } = 0;

            /// <summary>
            /// Gets or sets the type of token.
            /// </summary>
            [JsonProperty("token_type")]
            public string TokenType { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the username associated with the token.
            /// </summary>
            [JsonProperty("user_name")]
            public string UserName { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the issued time of the token.
            /// </summary>
            [JsonProperty(".issued")]
            public string Issued { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the expiration time of the token.
            /// </summary>
            [JsonProperty(".expires")]
            public string Expires { get; set; } = string.Empty;
        }
    }
}
