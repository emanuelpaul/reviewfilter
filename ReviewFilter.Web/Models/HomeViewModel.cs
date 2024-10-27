using ReviewFilter.ThirdParty.OpenApi.Models;

namespace ReviewFilter.Web.Models
{
    public class HomeViewModel
    {
        public string? ReviewContent { get; set; }

        public VerificationResult? VerificationResult { get; set; }

        public string? MLResult { get; set; }
        
        public int ExaggeratedWordsCount { get; set; }

        public int SimilarReviewsCount { get; set; }

        public string? Conclusion {  get; set; }

        public decimal FakeScore { get; set; }
    }
}
