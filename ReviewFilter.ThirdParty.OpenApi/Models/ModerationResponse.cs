using Newtonsoft.Json;

namespace ReviewFilter.ThirdParty.OpenApi.Models
{
    internal class ModerationResponse
    {
        public string? id { get; set; }

        public string? model { get; set; }

        public List<ModerationResult>? results { get; set; }
    }
}
