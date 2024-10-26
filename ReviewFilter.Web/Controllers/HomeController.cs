using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
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
        model.VerificationResult = await verificationContentEngine.Verify(model.InputContent);
        model.SimilarReviewsCount = 0;
        foreach (string dbReview in dbContext.NewReviews.Select( x => x.ReviewText))
        {
            var score = await verificationContentEngine.VerifySimilarities(model.InputContent ?? "", dbReview);
            if(score > 0.90)
                model.SimilarReviewsCount++;
        }

        model.MLResult = machineLearningService.Analize(model.InputContent);
        dbContext.NewReviews.Add(new NewReview { ReviewText = model.InputContent });
        dbContext.SaveChanges();

        string cleanedInput = CleanInput(model.InputContent);
        model.ExaggeratedWordsCount = 0;
        foreach (string exaggeratedWord in configuration.GetSection("exaggeratedWords").Get<string[]>())
        {
            if (cleanedInput.Contains(exaggeratedWord))
            {
                model.ExaggeratedWordsCount++;
            }
        }

        // Return the updated model to the view
        return View("Index", model);
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