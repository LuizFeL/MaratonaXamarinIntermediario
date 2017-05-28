using System;
using NewsCentralizer.ViewModel;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.View
{
    public partial class SocialLoginView : ContentPage
    {
        private SocialLoginViewModel ViewModel => BindingContext as SocialLoginViewModel;

        public SocialLoginView()
        {
            InitializeComponent();
            App.CurrentUser = null;
            ToolbarItems.Add(new ToolbarItem("Usuário", "usericon.png", () => { }, ToolbarItemOrder.Primary));
            var service = DependencyService.Get<IFelApiService>();
            BindingContext = new SocialLoginViewModel(service);
        }
    }
}
