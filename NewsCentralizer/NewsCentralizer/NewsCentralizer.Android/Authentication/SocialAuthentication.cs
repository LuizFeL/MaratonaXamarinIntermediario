using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NewsCentralizer.Authentication;
using NewsCentralizer.Droid.Authentication;
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