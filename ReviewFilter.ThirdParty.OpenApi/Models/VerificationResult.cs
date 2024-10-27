using Newtonsoft.Json;

namespace ReviewFilter.ThirdParty.OpenApi.Models
{
    public class VerificationResult
    {
        public bool Success { get; internal set; }
        public string? ErrorMessage { get; internal set; }
        public string? StackTrace { get; internal set; }
        public decimal SexualContent { get; set; }
        public decimal HateContent { get; set; }
        public decimal HarassmentContent { get; set; }
        public string? Sentiment { get; internal set; }
    }
}
