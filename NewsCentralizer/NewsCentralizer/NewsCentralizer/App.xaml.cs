using System;
using NewsCentralizer.Model;
using NewsCentralizer.View;
using Xamarin.Forms;

namespace NewsCentralizer
{
    public partial class App : Application
    {
        public static UserInfoModel UserInfo { get; set; }

        public App()
        {
            try
            {
                InitializeComponent();
                MainPage = new NavigationPage(new SocialLoginView());
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
