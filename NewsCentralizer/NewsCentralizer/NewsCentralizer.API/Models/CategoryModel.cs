using Newtonsoft.Json;

namespace NewsCentralizer.API.Models
{
    public class CategoryModel : BaseModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        public string Version { get; set; }
    }
}
