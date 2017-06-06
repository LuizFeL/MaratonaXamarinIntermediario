using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace NewsCentralizer.API.Models
{
    public class BaseModel : IKeyObject
    {
        [JsonProperty("Id")]
        [Key]
        public string Id { get; set; }
    }
}
