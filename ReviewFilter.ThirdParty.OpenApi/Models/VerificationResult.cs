using Newtonsoft.Json;

namespace ReviewFilter.ThirdParty.OpenApi.Models
{
    public class VerificationResult
    {
        public bool Success { get; internal set; }
        public string? ErrorMessage { get; internal set; }
        public string? StackTrace { get; internal set; }
        public double SexualContent { get; set; }
        public double HateContent { get; set; }
        public double HarassmentContent { get; set; }
    }
}
