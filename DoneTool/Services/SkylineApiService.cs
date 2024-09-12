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
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using Skyline.DataMiner.Utils.JsonOps.Models;

    public class SkylineApiService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<SkylineApiService> logger;
        private readonly SkylineApiData options;
        private string accessToken;
        private string refreshToken;

        public SkylineApiService(HttpClient httpClient, ILogger<SkylineApiService> logger, IOptions<SkylineApiData> options)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.options = options.Value;
        }

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
            this.accessToken = tokenData.AccessToken;
            this.refreshToken = tokenData.RefreshToken;

            this.logger.LogInformation("Access token acquired: {accessToken}", this.accessToken);

            return this.accessToken;
        }

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
            return taskDataList.FirstOrDefault() ?? throw new Exception("No task data returned from API.");
        }


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

        private class TokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("user_name")]
            public string UserName { get; set; }

            [JsonProperty(".issued")]
            public string Issued { get; set; }

            [JsonProperty(".expires")]
            public string Expires { get; set; }
        }
    }
}
