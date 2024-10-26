﻿using ReviewFilter.ThirdParty.OpenApi.Clients;
using ReviewFilter.ThirdParty.OpenApi.Models;

namespace ReviewFilter.ThirdParty.OpenApi.Engines
{
    public class VerificationContentEngine
    {
        private readonly string _apiKey;

        public VerificationContentEngine(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<VerificationResult> Verify(string content)
        {
            var moderationClient = new OpenAIModerationClient(_apiKey);

            try
            {
                var moderationResult = await moderationClient.ModerateContentAsync("Test content");

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
}