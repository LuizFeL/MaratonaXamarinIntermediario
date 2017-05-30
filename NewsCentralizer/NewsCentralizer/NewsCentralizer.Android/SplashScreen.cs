using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace NewsCentralizer.Droid
{
    [Activity(Label = "FeL - News Centralizer", Icon = "@drawable/icon", MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);

                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}