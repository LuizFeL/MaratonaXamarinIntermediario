using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace NewsCentralizer.Model
{
    [DataTable("User")]
    public class UserModel : ObservableBaseObject, IKeyObject
    {
        private string _socialLoginToken;
        private string _nome;
        private string _avatar;
        private string _id;

        [PrimaryKey]
        [JsonProperty("Id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        [JsonProperty("SocialLoginToken")]
        public string SocialLoginToken
        {
            get { return _socialLoginToken; }
            set { _socialLoginToken = value; OnPropertyChanged(); }
        }

        [JsonProperty("Nome")]
        public string Nome
        {
            get { return _nome; }
            set { _nome = value; OnPropertyChanged(); }
        }

        [JsonProperty("Avatar")]
        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; OnPropertyChanged(); }
        }

        [Version]
        public string Version { get; set; }
    }
}
