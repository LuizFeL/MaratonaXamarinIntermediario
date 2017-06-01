using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace NewsCentralizer.Model
{
    [DataTable("Category")]
    public class CategoryModel : BaseModel
    {
        private string _name;

        [JsonProperty("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        [Version]
        public string Version { get; set; }
    }
}
