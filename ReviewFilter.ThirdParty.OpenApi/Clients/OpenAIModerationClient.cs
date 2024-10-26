using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ReviewFilter.ThirdParty.OpenApi.Models;

namespace ReviewFilter.ThirdParty.OpenApi.Clients
{
    internal class OpenAIModerationClient(HttpClient httpClient)
    {
        private readonly string _apiKey;

        public async Task<ModerationResponse> ModerateContentAsync(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be null or empty", nameof(content));
            }

            var moderationRequest = new
            {
                input = content
            };
            var response = await httpClient.PostAsJsonAsync("v1/moderations", moderationRequest);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ModerationResponse>();
            }
            else
            {
                throw new HttpRequestException(
                    $"Moderation API request failed with status code: {response.StatusCode}");
            }
            //}
        }
    }
}