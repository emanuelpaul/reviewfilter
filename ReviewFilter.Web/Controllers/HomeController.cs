using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReviewFilter.ThirdParty.OpenApi.Engines;
using ReviewFilter.ThirdParty.OpenApi.Models;
using ReviewFilter.Web.Models;

namespace ReviewFilter.Web.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new HomeViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> VerifyContent(HomeViewModel model)
    {
        var apiKey = _configuration["ApiKey"]!;

        var verificationContentEngine = new VerificationContentEngine(apiKey);

        model.VerificationResult = await verificationContentEngine.Verify(model.InputContent);

        // Return the updated model to the view
        return View("Index", model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}