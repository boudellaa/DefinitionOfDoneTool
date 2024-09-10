// <copyright file="SkylineApiService.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DoneTool.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using DoneTool.Models.DTO;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

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
                this.accessToken = await GetAccessTokenAsync();
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
