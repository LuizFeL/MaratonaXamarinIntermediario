using System.Collections.Generic;
using System.Collections.ObjectModel;
using NewsCentralizer.Model;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.ViewModel
{
    public class SocialLoginViewModel : BaseViewModel
    {
        private readonly IFelApiService _service;

        public SocialLoginViewModel(IFelApiService service)
        {
            _service = service;
            Title = "Centralizador de notícias";
            var loginsTypeList = new List<SocialLoginModel>
            {
                new SocialLoginModel
                {
                    BackgroundColor = Color.FromHex("#ba3a2a"),
                    TextColor = Color.White,
                    Name = "Google +",
                    Logo = "google.png",
                    Type = SocialLoginType.Google
                },
                new SocialLoginModel
                {
                    BackgroundColor = Color.FromHex("#daf5fd"),
                    TextColor = Color.FromHex("#0078ff"),
                    Name = "Windows",
                    Logo = "windows.png",
                    Type = SocialLoginType.Windows
                },
                new SocialLoginModel
                {
                    BackgroundColor = Color.FromHex("#677ca7"),
                    TextColor = Color.White,
                    Name = "Facebook",
                    Logo = "facebook.png",
                    Type = SocialLoginType.Facebook
                },
                new SocialLoginModel
                {
                    BackgroundColor = Color.FromHex("#268ed7"),
                    TextColor = Color.White,
                    Name = "Twitter",
                    Logo = "twitter.png",
                    Type = SocialLoginType.Twitter
                }
            };
            SocialLogins = new ObservableCollection<SocialLoginModel>(loginsTypeList);
        }

        public ObservableCollection<SocialLoginModel> SocialLogins { get; set; }
    }
}
