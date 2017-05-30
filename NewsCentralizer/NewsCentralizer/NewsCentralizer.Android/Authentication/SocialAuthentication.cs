using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using NewsCentralizer.Authentication;
using NewsCentralizer.Droid.Authentication;
using NewsCentralizer.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(SocialAuthentication))]
namespace NewsCentralizer.Droid.Authentication
{
    public class SocialAuthentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client,
            MobileServiceAuthenticationProvider provider,
            IDictionary<string, string> parameters = null)
        {
            var user = await client.LoginAsync(Forms.Context, provider);
            return user;
        }
    }
}