using System;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace NewsCentralizer.Model
{
    public class UserInfoModel : BaseModel
    {
        private string _name;
        private string _image;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged(); }
        }

    }
}
