using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewFilter.ThirdParty.MachineLearning;
using ReviewFilter.ThirdParty.OpenApi.Engines;
using ReviewFilter.ThirdParty.OpenApi.Models;
using ReviewFilter.Web.Models;
using ReviewFilter.Web.NewReviewsStorage;

namespace ReviewFilter.Web.Controllers;

public class HomeController(
    IVerificationContentEngine verificationContentEngine,
    IMachineLearningService machineLearningService,
    NewReviewsDbContext dbContext,
    IConfiguration configuration) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new HomeViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> VerifyContent(HomeViewModel model)
    {
        model.VerificationResult = await verificationContentEngine.Verify(model.ReviewContent);

        if (model.VerificationResult.Success)
        {
            model.SimilarReviewsCount = 0;
            foreach (string dbReview in dbContext.NewReviews.Select(x => x.ReviewText))
            {
                var score = await verificationContentEngine.VerifySimilarities(model.ReviewContent ?? "", dbReview);
                if (score > 0.90)
                    model.SimilarReviewsCount++;
            }

            model.MLResult = machineLearningService.Analize(model.ReviewContent);
            dbContext.NewReviews.Add(new NewReview { ReviewText = model.ReviewContent });
            dbContext.SaveChanges();

            string cleanedInput = CleanInput(model.ReviewContent);
            model.ExaggeratedWordsCount = 0;
            foreach (string exaggeratedWord in configuration.GetSection("exaggeratedWords").Get<string[]>())
            {
                if (cleanedInput.Contains(exaggeratedWord))
                {
                    model.ExaggeratedWordsCount++;
                }
            }

            model.Conclusion = GetConclusion(model);
        }

        // Return the updated model to the view
        return View("Index", model);
    }

    private string? GetConclusion(HomeViewModel model)
    {
        decimal fakeScore = (decimal)0.0;

        // Weights for different types of fake reviews
        // 1. Fabricated Positive Review (Highly positive sentiment with CG or exaggerated words or similar reviews)
        if (model.VerificationResult!.Sentiment!.Contains("positive") && (model.MLResult == "CG" || model.ExaggeratedWordsCount > 2 || model.SimilarReviewsCount > 3))
        {
            fakeScore += (decimal)0.2; // Add to fake score if it's too positive and exaggerated
        }

        // 2. Fabricated Negative Review (Highly negative sentiment with CG or exaggerated words or similar reviews)
        if (model.VerificationResult.Sentiment.Contains("negative") && (model.MLResult == "CG" || model.ExaggeratedWordsCount > 2 || model.SimilarReviewsCount > 3))
        {
            fakeScore += (decimal)0.2; // Add to fake score if it's too negative and exaggerated
        }

        // ML Result contributes
        if (model.MLResult == "CG")
        {
            fakeScore += (decimal)0.4;  // ML result contributes 40% to the final score if "CG"
        }

        // Check content flags (Sexual, Hate, Harassment) - they indicate suspicious content
        decimal contentThreshold = (decimal)0.01; // Example threshold
        if (model.VerificationResult.SexualContent > contentThreshold) fakeScore += (decimal)0.1;
        if (model.VerificationResult.HateContent > contentThreshold) fakeScore += (decimal)0.1;
        if (model.VerificationResult.HarassmentContent > contentThreshold) fakeScore += (decimal)0.1;

        // Final decision based on fakeScore threshold
        decimal fakeThreshold = (decimal)0.5; // Threshold for fakeness

        model.FakeScore = fakeScore;

        if (fakeScore > fakeThreshold)
        {
            return $"The review is likely to be FAKE in proportion of {model.FakeScore*100} %.";
        }
        else
        {
            return $"The review appears to be VALID in proportion of {(int)(100 -(model.FakeScore * 100))} %.";
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private string CleanInput(string input)
    {
        return Regex.Replace(input, @"[^a-zA-Z0-9\s]", "").ToLower();
    }
}