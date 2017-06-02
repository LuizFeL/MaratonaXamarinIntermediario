using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace NewsCentralizer.Model
{
    [DataTable("Preference")]
    public class PreferenceModel : BaseModel
    {
        private string _userId;
        private string _categoryId;
        private string _tag;

        [JsonProperty("UserId")]
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(); }
        }

        [JsonProperty("CategoryId")]
        public string CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public CategoryModel Category { get; set; }

        [JsonProperty("Tag")]
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public int Index { get; set; }


        [Version]
        public string Version { get; set; }
    }
}
