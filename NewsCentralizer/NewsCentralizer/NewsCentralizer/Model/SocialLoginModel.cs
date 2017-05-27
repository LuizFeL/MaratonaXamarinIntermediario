using Xamarin.Forms;

namespace NewsCentralizer.Model
{
    public enum SocialLoginType { Google, Windows, Facebook, Twitter }
    public class SocialLoginModel
    {
        public string Name { get; set; }
        public ImageSource Logo { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public SocialLoginType Type { get; set; }
    }
}
