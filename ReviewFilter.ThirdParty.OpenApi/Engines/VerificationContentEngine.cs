using Newtonsoft.Json;
using ReviewFilter.ThirdParty.OpenApi.Clients;
using ReviewFilter.ThirdParty.OpenApi.Models;
using System.Text;

namespace ReviewFilter.ThirdParty.OpenApi.Engines
{
    internal class VerificationContentEngine(OpenAIClient openAiClient) : IVerificationContentEngine
    {
        public async Task<VerificationResult> Verify(string? reviewContent)
        {
            try
            {
                var moderationResult = await openAiClient.ModerateContentAsync(reviewContent);

                if (moderationResult == null || 
                    moderationResult.results == null || 
                    moderationResult.results[0].category_scores == null)
                {
                    return new VerificationResult
                    {
                        Success = false,
                        ErrorMessage = "Moderation result is null."
                    };
                }

                var categoryScores = moderationResult.results![0].category_scores!;

                return new VerificationResult
                {
                    Success = true,
                    SexualContent = categoryScores.sexual,
                    HarassmentContent = categoryScores.harassment,
                    HateContent = categoryScores.hate
                };
            }
            catch (Exception ex)
            {
                return new VerificationResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }

        public async Task<double> VerifySimilarities(string text1, string text2)
        {
            try
            {
                var embedding1 = await openAiClient.GetEmbedding(text1);
                var embedding2 = await openAiClient.GetEmbedding(text2);

                // Calculate cosine similarity between the embeddings
                return CosineSimilarity(embedding1, embedding2);
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        private static double CosineSimilarity(double[] vectorA, double[] vectorB)
        {
            double dotProduct = vectorA.Zip(vectorB, (a, b) => a * b).Sum();
            double magnitudeA = Math.Sqrt(vectorA.Sum(a => a * a));
            double magnitudeB = Math.Sqrt(vectorB.Sum(b => b * b));

            return dotProduct / (magnitudeA * magnitudeB);
        }
    }



    public interface IVerificationContentEngine
    {
        Task<VerificationResult> Verify(string? reviewContent);

        Task<double> VerifySimilarities(string text1, string text2);
    }
}
