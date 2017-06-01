namespace NewsCentralizer.Model
{
    public class UserInfoModel : BaseModel
    {
        private string _name;
        private string _imageUri;
        
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
