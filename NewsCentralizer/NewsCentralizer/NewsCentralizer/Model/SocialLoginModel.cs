using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace NewsCentralizer.Model
{
    public class SocialLoginModel
    {
        public string Name { get; set; }
        public ImageSource Logo { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public MobileServiceAuthenticationProvider Provider { get; set; }
    }
}
