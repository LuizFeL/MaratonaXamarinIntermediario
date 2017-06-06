using Newtonsoft.Json;

namespace NewsCentralizer.API.Models
{
    public class NewsModel : BaseModel
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public int Index { get; set; }

        public string Version { get; set; }
    }
}
