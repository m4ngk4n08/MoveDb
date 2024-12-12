using Microsoft.Extensions.Options;
using MoveDb.Services.Data.Entities;
using MoveDb.Services.Services.IServices;
using RestSharp;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MoveDb.Services.Services {
    public class ApiService : IApiService {
        private readonly APISettings _apiSettings;

        public ApiService(
            IOptionsSnapshot<AppSettings> optionAccessor)
        {
            _apiSettings = optionAccessor.Value.APISettings;
        }

        public async Task<string> APIRequests(string url)
        {
            var options = new RestClientOptions(url);
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", _apiSettings.TMDBAPISettings.BaseAppSettings.BearerToken);
            var response = await client.GetAsync(request);

            return response.Content ?? string.Empty;
        }

        public async Task<string?> GeminiApiRequest(GemeniRequest gemeniRequest)
        {
            string jsonOutput = JsonSerializer.Serialize(gemeniRequest);

            using var client = new HttpClient();
            var geminiApiSet = _apiSettings.GeminiApi;
            var request = new HttpRequestMessage(HttpMethod.Post, string.Format("{0}{1}", geminiApiSet.APIUrlGemini, geminiApiSet.APIKeyGemini));

            request.Content = new StringContent(jsonOutput, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request).ConfigureAwait(false);
            var responseBody = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new ArgumentException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }

            return responseBody;
        }
    }
}
