using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace NewsCentralizer.Model
{
    [DataTable("Favorite")]
    public class FavoriteModel : ObservableBaseObject, IKeyObject
    {
        private string _id;
        private string _userId;
        private string _newsId;

        [PrimaryKey]
        [JsonProperty("Id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        [JsonProperty("UserId")]
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public UserModel User { get; set; }

        [JsonProperty("NewsId")]
        public string NewsId
        {
            get { return _newsId; }
            set { _newsId = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public NewsModel News { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
