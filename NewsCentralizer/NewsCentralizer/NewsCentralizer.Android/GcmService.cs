using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;
using NewsCentralizer.Model;
using NewsCentralizer.View;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
namespace NewsCentralizer.Droid
{
    [BroadcastReceiver(Permission = Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new[] { Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new[] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new[] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new[] { "@PACKAGE_NAME@" })]
    public class PushHendlerBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SenderIDs = { "32832165340" };
    }

    [Service]
    public class GcmService : GcmServiceBase
    {
        MobileServiceClient client = new MobileServiceClient(Helpers.Constants.AppUrl);
        public static string RegistrationId { get; private set; }

        public GcmService() : base(PushHendlerBroadcastReceiver.SenderIDs) { }

        protected override void OnRegistered(Context context, string registrationId)
        {
            RegistrationId = registrationId;
            var push = client.GetPush();
            MainActivity.CurrentActivity.RunOnUiThread(() => Register(push, null));
        }

        private async void Register(Push push, IEnumerable<string> tags)
        {
            try
            {
                const string templateBodyGcm = "{\"data\":{\"message\":\"$(messageParam)\"}}";

                var templates = new JObject();
                templates["genericMessage"] = new JObject { { "body", templateBodyGcm } };

                await push.RegisterAsync(RegistrationId, templates);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            try
            {
                var msg = new StringBuilder();
                if (intent != null && intent.Extras != null)
                    foreach (var key in intent.Extras.KeySet())
                        msg.AppendLine(key + "=" + intent.Extras.Get(key));

                var prefs = GetSharedPreferences(context.PackageName, FileCreationMode.Private);
                var edit = prefs.Edit();
                edit.PutString("last_msg", msg.ToString());
                edit.Commit();

                var message = intent?.Extras?.GetString("message");
                if (!string.IsNullOrWhiteSpace(message))
                {
                    CreateNotification("Notícia", message);
                    return;
                }

                var message2 = intent?.Extras?.GetString("msg");

                if (string.IsNullOrWhiteSpace(message2)) return;

                CreateNotification("Notícia!", message2);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " | " + ex.StackTrace);
                App.Current?.MainPage?.DisplayAlert("Erro na Mensagem", ex.Message, "OK");
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {

        }

        protected override void OnError(Context context, string errorId)
        {
            try
            {
                App.Current?.MainPage?.DisplayAlert("Erro Notification", errorId, "OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void CreateNotification(string title, string msg)
        {
            var news = JsonConvert.DeserializeObject<NewsModel>(msg);
            App.NewsNotification = news;
            var notifManager = GetSystemService(NotificationService) as NotificationManager;
            var uiIntent = new Intent(this, typeof(MainActivity));
            var builder = new NotificationCompat.Builder(this);

            var notification = builder.SetContentIntent(PendingIntent.GetActivity(this, 0, uiIntent, 0))
                .SetSmallIcon(Resource.Drawable.news)
                .SetTicker(title)
                .SetContentTitle(title)
                .SetContentText(news.Title)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetAutoCancel(true)
                .Build();

            notifManager?.Notify(1, notification);
        }
    }
}