using Newtonsoft.Json;

namespace ReviewFilter.ThirdParty.OpenApi
{
    public class ModerationResponse
    {
        public string? id { get; set; }

        public string? model { get; set; }

        public List<ModerationResult>? results { get; set; }
    }
}
