using Newtonsoft.Json;

namespace ReviewFilter.ThirdParty.OpenApi
{
    public class ModerationResult
    {
        public bool flagged { get; set; }
        public Categories? categories { get; set; }
        public CategoryScores? category_scores { get; set; }
    }
}
