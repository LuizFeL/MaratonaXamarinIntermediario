using System;
using NewsCentralizer.Helpers;
using NewsCentralizer.Model;
using NewsCentralizer.Services;
using NewsCentralizer.View;
using Xamarin.Forms;

namespace NewsCentralizer
{
    public partial class App : Application
    {
        public static UserInfoModel UserInfo { get; set; }
        public static AzureClient AzureClient { get; private set; }
        public static NewsModel NewsNotification { get; set; }

        public App()
        {
            try
            {
                InitializeComponent();
                AzureClient = new AzureClient();
                SetMainPage();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public static void SetMainPage()
        {
            var page = NewsNotification != null
                      ? new NewsView(NewsNotification)
                      : AzureClient.TryLogin() ? (Page)new TopNewsView() : new SocialLoginView(AzureClient);
            App.Current.MainPage = new NavigationPage(page);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
