using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace NewsCentralizer.Model
{
    [DataTable("Preference")]
    public class PreferenceModel : ObservableBaseObject, IKeyObject
    {
        private string _id;
        private string _userId;
        private string _categoryId;
        private string _tag;

        [PrimaryKey]
        [JsonProperty("Id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public UserModel User { get; set; }

        public string CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public CategoryModel Category { get; set; }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; OnPropertyChanged(); }
        }

        [Version]
        public string Version { get; set; }
    }
}
