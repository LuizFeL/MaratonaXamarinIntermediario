using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace NewsCentralizer.Model
{
    [DataTable("News")]
    public class NewsModel : BaseModel
    {
        private string _url;
        private string _title;
        private string _imageUrl;
       
        [JsonProperty("Title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        [JsonProperty("Url")]
        public string Url
        {
            get { return _url; }
            set { _url = value; OnPropertyChanged(); }
        }

        [JsonProperty("ImageUrl")]
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public int Index { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
