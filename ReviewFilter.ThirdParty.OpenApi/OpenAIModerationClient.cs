using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ReviewFilter.ThirdParty.OpenApi
{
    public class OpenAIModerationClient
    {
        private readonly string _apiKey;
        private static readonly string moderationEndpoint = "https://api.openai.com/v1/moderations";

        public OpenAIModerationClient(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public async Task<ModerationResponse> ModerateContentAsync(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be null or empty", nameof(content));
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var moderationRequest = new
                {
                    input = content
                };

                var requestBody = new StringContent(JsonSerializer.Serialize(moderationRequest), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(moderationEndpoint, requestBody);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<ModerationResponse>(jsonResponse)!;
                }
                else
                {
                    throw new HttpRequestException($"Moderation API request failed with status code: {response.StatusCode}");
                }
            }
        }
    }
}
