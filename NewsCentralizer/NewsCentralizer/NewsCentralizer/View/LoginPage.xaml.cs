using System;
using NewsCentralizer.ViewModel;
using Xamarin.Forms;

namespace NewsCentralizer.View
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            ToolbarItems.Add(new ToolbarItem("Usuário", "usericon.png", () => { }, ToolbarItemOrder.Primary));
            BindingContext = new SocialLoginViewModel();
        }
    }
}
