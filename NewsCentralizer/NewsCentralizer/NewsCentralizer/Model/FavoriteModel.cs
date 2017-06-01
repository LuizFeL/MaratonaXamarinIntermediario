using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace NewsCentralizer.Model
{
    [DataTable("Favorite")]
    public class FavoriteModel : BaseModel
    {
        private string _userId;
        private string _newsId;
        
        [JsonProperty("UserId")]
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(); }
        }
        
        [JsonProperty("NewsId")]
        public string NewsId
        {
            get { return _newsId; }
            set { _newsId = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public NewsModel News { get; set; }

        [JsonIgnore]
        public int Index { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
