using Newtonsoft.Json;
using SQLite;

namespace NewsCentralizer.Model
{
    public class BaseModel : ObservableBaseObject, IKeyObject
    {
        private string _id;

        [PrimaryKey]
        [JsonProperty("Id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
    }
}
