using NewsCentralizer.ViewModel;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.View
{
    public partial class SocialLoginView
    {
        //private SocialLoginViewModel ViewModel => BindingContext as SocialLoginViewModel

        public SocialLoginView()
        {
            InitializeComponent();
            ToolbarItems.Add(new ToolbarItem("Usuário", "usericon.png", () => { }, ToolbarItemOrder.Primary));
            var service = DependencyService.Get<IFelApiService>();
            BindingContext = new SocialLoginViewModel(service);
            Appearing += (sender, e) => { App.CurrentUser = null; };
        }
    }
}
