using NewsCentralizer.View;
using Xamarin.Forms;

namespace NewsCentralizer
{
    public partial class App : Application
    {
        //public static User CurrentUser { get; set; }

        public static void LoginAction()
        {
            
        } 

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
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
