using ReviewFilter.ThirdParty.OpenApi.Clients;
using ReviewFilter.ThirdParty.OpenApi.Models;

namespace ReviewFilter.ThirdParty.OpenApi.Engines
{
    internal class VerificationContentEngine(OpenAIModerationClient moderationClient) : IVerificationContentEngine
    {
        public async Task<VerificationResult> Verify(string? content)
        {
            try
            {
                var moderationResult = await moderationClient.ModerateContentAsync(content);

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
    }

    public interface IVerificationContentEngine
    {
        Task<VerificationResult> Verify(string? content);
    }
}
