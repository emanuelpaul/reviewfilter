using ReviewFilter.ThirdParty.OpenApi.Models;

namespace ReviewFilter.Web.Models
{
    public class HomeViewModel
    {
        public string? InputContent { get; set; }
        public VerificationResult? VerificationResult { get; set; }
    }
}
