using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using ReviewFilter.ThirdParty.OpenApi.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ReviewFilter.ThirdParty.OpenApi.Clients
{
    internal class OpenAIClient(HttpClient httpClient)
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

        public async Task<double[]> GetEmbedding(string text)
        {
            var requestBody = new
            {
                model = "text-embedding-ada-002",
                input = text
            };

            var response = await httpClient.PostAsJsonAsync("v1/embeddings", requestBody);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(jsonResponse);

            // Check for errors
            if (result.error != null)
            {
                throw new Exception(result.error.message.ToString());
            }

            // Extract embedding vector as an array of doubles
            return ((IEnumerable<dynamic>)result.data[0].embedding).Select(x => (double)x).ToArray();
        }
    }
}