using Newtonsoft.Json;

namespace ReviewFilter.ThirdParty.OpenApi.Models
{
    internal class CategoryScores
    {
        public decimal sexual { get; set; }
        public decimal hate { get; set; }
        public decimal harassment { get; set; }

        [JsonProperty("self-harm")]
        public decimal selfharm { get; set; }

        [JsonProperty("sexual/minors")]
        public decimal sexualminors { get; set; }

        [JsonProperty("hate/threatening")]
        public decimal hatethreatening { get; set; }

        [JsonProperty("violence/graphic")]
        public decimal violencegraphic { get; set; }

        [JsonProperty("self-harm/intent")]
        public decimal selfharmintent { get; set; }

        [JsonProperty("self-harm/instructions")]
        public decimal selfharminstructions { get; set; }

        [JsonProperty("harassment/threatening")]
        public decimal harassmentthreatening { get; set; }
        public decimal violence { get; set; }
    }
}
