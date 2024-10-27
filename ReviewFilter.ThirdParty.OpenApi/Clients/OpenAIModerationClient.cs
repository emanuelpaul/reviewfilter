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

        public async Task<ModerationResponse> ModerateContentAsync(string? reviewContent)
        {
            if (string.IsNullOrWhiteSpace(reviewContent))
            {
                throw new ArgumentException("Review cannot be null or empty", nameof(reviewContent));
            }

            var moderationRequest = new
            {
                input = reviewContent
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
        }

        public async Task<double[]> GetEmbedding(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Text cannot be null or empty", nameof(text));
            }

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

        public async Task<AnalyzerSentimentResponse> AnalyzeSentimentAsync(string? reviewContent)
        {
            if (string.IsNullOrWhiteSpace(reviewContent))
            {
                throw new ArgumentException("Review cannot be null or empty", nameof(reviewContent));
            }

            var analyzerSentimentRequest = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant that analyzes sentiment." },
                new { role = "user", content = $"Analyze the sentiment of the following text: \"{reviewContent}\". Is it positive, negative, or neutral?" }
            },
                max_tokens = 50,
                temperature = 0.7
            };
            var response = await httpClient.PostAsJsonAsync("v1/chat/completions", analyzerSentimentRequest);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AnalyzerSentimentResponse>();
            }
            else
            {
                throw new HttpRequestException(
                    $"Moderation API request failed with status code: {response.StatusCode}");
            }
        }
    }
}