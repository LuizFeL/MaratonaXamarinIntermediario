using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Gcm.Client;
using NewsCentralizer.View;

namespace NewsCentralizer.Droid
{
    [Activity(Label = "NewsCentralizer", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity CurrentActivity { get; private set; }      

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                CurrentActivity = this;

                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(bundle);

                Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

                Xamarin.Forms.Forms.Init(this, bundle);
                LoadApplication(new App());

                CheckGcm();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void CheckGcm()
        {
            try
            {
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);
                GcmClient.Register(this, PushHendlerBroadcastReceiver.SenderIDs);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /*protected override void OnNewIntent(Intent intent)
        {
            if (intent == null && App.NewsNotification!=null) return;

            var view = new NewsView(App.NewsNotification);
            App.Current?.MainPage?.Navigation.PushAsync(view);
        }*/
    }
}

