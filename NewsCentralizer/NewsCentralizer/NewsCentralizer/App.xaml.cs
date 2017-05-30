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

        public App()
        {
            try
            {
                InitializeComponent();
                AzureClient = new AzureClient();
                var page = AzureClient.TryLogin() ? (Page)new TopNewsView() : new SocialLoginView(AzureClient);


                MainPage = new NavigationPage(page);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
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
