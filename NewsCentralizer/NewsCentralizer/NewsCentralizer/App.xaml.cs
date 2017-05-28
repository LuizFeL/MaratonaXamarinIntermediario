using NewsCentralizer.Model;
using NewsCentralizer.View;
using Xamarin.Forms;

namespace NewsCentralizer
{
    public partial class App : Application
    {
        public static UserModel CurrentUser { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SocialLoginView());
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
