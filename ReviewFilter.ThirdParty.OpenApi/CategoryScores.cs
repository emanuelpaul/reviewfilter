using Newtonsoft.Json;

namespace ReviewFilter.ThirdParty.OpenApi
{
    public class CategoryScores
    {
        public double sexual { get; set; }
        public double hate { get; set; }
        public double harassment { get; set; }

        [JsonProperty("self-harm")]
        public double selfharm { get; set; }

        [JsonProperty("sexual/minors")]
        public double sexualminors { get; set; }

        [JsonProperty("hate/threatening")]
        public double hatethreatening { get; set; }

        [JsonProperty("violence/graphic")]
        public double violencegraphic { get; set; }

        [JsonProperty("self-harm/intent")]
        public double selfharmintent { get; set; }

        [JsonProperty("self-harm/instructions")]
        public double selfharminstructions { get; set; }

        [JsonProperty("harassment/threatening")]
        public double harassmentthreatening { get; set; }
        public double violence { get; set; }
    }
}
