namespace NewsCentralizer.Model
{
    public class UserInfoModel : ObservableBaseObject, IKeyObject
    {
        private string _id;
        private string _name;
        private string _imageUri;

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string ImageUri
        {
            get { return _imageUri; }
            set { _imageUri = value; OnPropertyChanged(); }
        }
    }
}
