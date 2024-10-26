using Newtonsoft.Json;

namespace ReviewFilter.ThirdParty.OpenApi.Models
{
    internal class Categories
    {
        public bool sexual { get; set; }
        public bool hate { get; set; }
        public bool harassment { get; set; }

        [JsonProperty("self-harm")]
        public bool selfharm { get; set; }

        [JsonProperty("sexual/minors")]
        public bool sexualminors { get; set; }

        [JsonProperty("hate/threatening")]
        public bool hatethreatening { get; set; }

        [JsonProperty("violence/graphic")]
        public bool violencegraphic { get; set; }

        [JsonProperty("self-harm/intent")]
        public bool selfharmintent { get; set; }

        [JsonProperty("self-harm/instructions")]
        public bool selfharminstructions { get; set; }

        [JsonProperty("harassment/threatening")]
        public bool harassmentthreatening { get; set; }
        public bool violence { get; set; }
    }
}
