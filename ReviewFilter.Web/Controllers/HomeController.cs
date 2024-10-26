using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReviewFilter.ThirdParty.MachineLearning;
using ReviewFilter.ThirdParty.OpenApi.Engines;
using ReviewFilter.ThirdParty.OpenApi.Models;
using ReviewFilter.Web.Models;
using ReviewFilter.Web.NewReviewsStorage;

namespace ReviewFilter.Web.Controllers;

public class HomeController(IVerificationContentEngine verificationContentEngine, IMachineLearningService machineLearningService,
    NewReviewsDbContext dbContext) : Controller
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
        model.SimilarityResult = await verificationContentEngine.VerifySimilarities(model.InputContent ?? "", "Acesta este un test");
        model.MLResult = machineLearningService.Analize(model.InputContent);
        dbContext.NewReviews.Add(new NewReview { ReviewText = model.InputContent });
        dbContext.SaveChanges();

        // Return the updated model to the view
        return View("Index", model);
    }    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}