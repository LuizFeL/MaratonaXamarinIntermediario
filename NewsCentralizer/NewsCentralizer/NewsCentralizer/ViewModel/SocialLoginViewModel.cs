using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NewsCentralizer.Helpers;
using NewsCentralizer.Model;
using Xamarin.Forms;
using NewsCentralizer.Services;
using Newtonsoft.Json.Linq;

namespace NewsCentralizer.ViewModel
{

    public class SocialLoginViewModel : BaseViewModel
    {
        private readonly IFelApiService _service;

        public SocialLoginViewModel(IFelApiService service)
        {
            _service = service;
            Title = "Centralizador de notícias";
            UserInfo = new UserInfoModel { Id = "0", ImageUri = "usericon.png", Name = "Fazer Login" };
            var loginsTypeList = new List<SocialLoginModel>
            {
                new SocialLoginModel
                {
                    BackgroundColor = Color.FromHex("#ba3a2a"),
                    TextColor = Color.White,
                    Name = "Google +",
                    Logo = "google.png",
                    Provider = MobileServiceAuthenticationProvider.Google
                },
                new SocialLoginModel
                {
                    BackgroundColor = Color.FromHex("#daf5fd"),
                    TextColor = Color.FromHex("#0078ff"),
                    Name = "Microsoft Account",
                    Logo = "windows.png",
                    Provider = MobileServiceAuthenticationProvider.MicrosoftAccount
                },
                new SocialLoginModel
                {
                    BackgroundColor = Color.FromHex("#677ca7"),
                    TextColor = Color.White,
                    Name = "Facebook",
                    Logo = "facebook.png",
                    Provider = MobileServiceAuthenticationProvider.Facebook
                },
                new SocialLoginModel
                {
                    BackgroundColor = Color.FromHex("#268ed7"),
                    TextColor = Color.White,
                    Name = "Twitter",
                    Logo = "twitter.png",
                    Provider = MobileServiceAuthenticationProvider.Twitter
                }
            };
            SocialLogins = new ObservableCollection<SocialLoginModel>(loginsTypeList);
            SocialLoginCommand = new Command<SocialLoginModel>(ExecuteSocialLoginCommand);
        }

        public ObservableCollection<SocialLoginModel> SocialLogins { get; set; }

        private UserInfoModel _userInfo;
        public UserInfoModel UserInfo
        {
            get { return _userInfo; }
            set { SetProperty(ref _userInfo, value); }
        }

        public Command<SocialLoginModel> SocialLoginCommand { get; }

        private async Task SetUserAvatar(SocialLoginModel socialLogin)
        {
            try
            {
                //TODO: Get user info
                UserInfo = new UserInfoModel { Id = "0", ImageUri = "usericon.png", Name = "Fazer Login" };
            }
            catch (Exception ex)
            {
                UserInfo = new UserInfoModel { Id = "0", ImageUri = "usericon.png", Name = "Fazer Login" };
                await DisplayAlert("Erro Avatar", ex.Message, "OK");
            }
            App.UserInfo = UserInfo;
        }

        private async void ExecuteSocialLoginCommand(SocialLoginModel socialLogin)
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(true);
                if (socialLogin == null) return;
                var logged = await new AzureClient<UserInfoModel>().LoginAsync(socialLogin);
                if (!logged)
                {
                    await DisplayAlert("Erro", "Não foi possível fazer login com " + socialLogin.Name, "OK");
                    return;
                }
                await Task.Run(() => SetUserAvatar(socialLogin));
                await DisplayAlert("Certo", "foi com " + socialLogin.Name, "OK");

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void ClearLoggedUser()
        {
            Settings.UserId = null;
            Settings.AuthToken = null;
            App.UserInfo = null;
            UserInfo = null;
        }

    }
}
